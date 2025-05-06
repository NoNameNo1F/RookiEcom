using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Exceptions;
using RookiEcom.Application.Storage;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Create;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ProductContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryCommandHandler(
        ProductContext dbContext, 
        IBlobService blobService,
        IValidator<CreateCategoryCommand> validator)
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _validator = validator;
    }

    public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        (string BlobName, string ContainerName)? uploadedBlob = null;

        try
        {
            var category = new Domain.CategoryAggregate.Category
            {
                Name = request.Name,
                Description = request.Description,
                HasChild = false,
                IsPrimary = request.IsPrimary,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
            
            if (request.ParentId.Value != 0)
            {
                category.ParentId = request.ParentId;
                var parentCategory = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.ParentId == request.ParentId.Value , cancellationToken);
                   
                if (parentCategory != null)
                {
                    parentCategory.HasChild = true;
                }
            }
            
            if (request.Image != null)
            {
                var blobName = Guid.NewGuid().ToString();
                var containerName = "images";
                var blobUri = await _blobService.UploadBlob(blobName, containerName, request.Image);
                if (string.IsNullOrEmpty(blobUri))
                {
                    throw new FailToUploadException();
                }
                category.Image = blobUri;
                uploadedBlob = (blobName, containerName);
            }
            
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            if (uploadedBlob.HasValue)
            {
                await _blobService.DeleteBlob(uploadedBlob.Value.BlobName, uploadedBlob.Value.ContainerName);
            }

            throw new ApplicationException($"Failed to create category: {ex.Message}", ex);
        }
    }
}