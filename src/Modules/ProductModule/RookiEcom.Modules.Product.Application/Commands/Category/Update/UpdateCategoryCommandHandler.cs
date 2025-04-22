using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Exceptions;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Update;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, int>
{
    private readonly ProductContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateCategoryCommandHandler(ProductContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        (string BlobName, string ContainerName)? newUploadedBlob = null;
        string oldImageUri = "";

        try
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            
            if (category == null)
            {
                throw new CategoryNotFoundException(request.Id);
            }

            if (request.Image != null && !string.IsNullOrEmpty(category.Image))
            {
                oldImageUri = category.Image;
                var blobName = new Uri(oldImageUri).Segments.Last();
                var containerName = "images";
                await _blobService.DeleteBlob(blobName, containerName);

                blobName = Guid.NewGuid().ToString();
                var blobUri = await _blobService.UploadBlob(blobName, containerName, request.Image);
                if (string.IsNullOrEmpty(blobUri))
                {
                    throw new FailToUploadException();
                }
                category.Image = blobUri;
                newUploadedBlob = (blobName, containerName);
            }

            category.Name = request.Name;
            category.Description = request.Description;
            var oldParentId = category.ParentId;
            category.ParentId = request.ParentId;
            category.IsPrimary = request.IsPrimary;
            category.UpdatedDateTime = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (oldParentId != request.ParentId)
            {
                //Update Category of Old ParentId before update parentId of new category
                if (oldParentId.HasValue)
                {
                    var oldParent = await _dbContext.Categories
                        .FirstOrDefaultAsync(c => c.ParentId == oldParentId.Value , cancellationToken);
                    
                    if (oldParent != null)
                    {
                        oldParent.HasChild = await _dbContext.Categories.AnyAsync(c => c.ParentId == oldParentId.Value, cancellationToken);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                //Update Category ParentId if it is changed
                if (request.ParentId.HasValue)
                {
                    var newParent = await _dbContext.Categories
                        .FirstOrDefaultAsync(c => c.ParentId == request.ParentId.Value , cancellationToken);
                   
                    if (newParent != null)
                    {
                        newParent.HasChild = true;
                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            await transaction.CommitAsync(cancellationToken);
            return category.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            if (newUploadedBlob.HasValue)
            {
                await _blobService.DeleteBlob(newUploadedBlob.Value.BlobName, newUploadedBlob.Value.ContainerName);
            }

            throw new ApplicationException($"Failed to update category: {ex.Message}", ex);
        }
    }
}