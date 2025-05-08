using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Exceptions;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Commands.Product.Create;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.Modules.Product.Application.Commands.ProductRating.Create;

public class CreateProductRatingCommandHandler : ICommandHandler<CreateProductRatingCommand>
{
    private readonly ProductContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IValidator<CreateProductRatingCommand> _validator;

    public CreateProductRatingCommandHandler(
        ProductContext dbContext, 
        IBlobService blobService, 
        IValidator<CreateProductRatingCommand> validator)
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _validator = validator;
    }
    
    public async Task Handle(CreateProductRatingCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var productExists = await _dbContext.Products
            .AnyAsync(p => p.Id == request.ProductId, cancellationToken);
        if (!productExists)
        {
            throw new ProductNotFoundException(request.ProductId);
        }
        
        
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        string? uploadedBlobUri = null;
        (string BlobName, string ContainerName)? uploadedBlobInfo = null;

        try
        {
            var rating = new Domain.ProductRatingAggregate.ProductRating
            {
                ProductId = request.ProductId,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                Score = request.Score,
                Content = request.Content,
                Image = "",
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };

            if (request.Image != null)
            {
                var blobName = Guid.NewGuid().ToString();
                var containerName = "review-images"; 
                uploadedBlobUri = await _blobService.UploadBlob(blobName, containerName, request.Image);
                if (string.IsNullOrEmpty(uploadedBlobUri))
                {
                    throw new FailToUploadException();
                }
                rating.Image = uploadedBlobUri;
                uploadedBlobInfo = (blobName, containerName);
            }

            _dbContext.ProductRatings.Add(rating);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            if (uploadedBlobInfo.HasValue)
            {
                await _blobService.DeleteBlob(uploadedBlobInfo.Value.BlobName, uploadedBlobInfo.Value.ContainerName);
            }

            throw;
        }
    }
}