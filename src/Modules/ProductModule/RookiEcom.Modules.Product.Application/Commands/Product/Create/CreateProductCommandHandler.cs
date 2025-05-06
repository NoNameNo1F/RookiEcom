using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Exceptions;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;
using RookiEcom.Modules.Product.Domain.Shared;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Create;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly ProductContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IValidator<CreateProductCommand> _validator;
    
    public CreateProductCommandHandler(
        ProductContext dbContext,
        IBlobService blobService,
        IValidator<CreateProductCommand> validator)
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _validator = validator;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        var uploadedBlobs = new List<(string BlobName, string ContainerName)>();

        if (await _dbContext.Products.AnyAsync(p => p.SKU == request.SKU, cancellationToken))
        {
            throw new ProductSKUExistedException(request.SKU);
        }
        
        try
        {
            var product = new Domain.ProductAggregate.Product
            {
                SKU = request.SKU,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                MarketPrice = request.MarketPrice,
                Price = request.Price,
                Status = ProductStatus.Available,
                StockQuantity = request.StockQuantity,
                IsFeature = request.IsFeature,
                ProductAttributes = request.ProductAttributes,
                ProductOption = request.ProductOption,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                Images = new List<string>()
            };
        
            foreach (var image in request.Images)
            {
                var blobName = Guid.NewGuid().ToString();
                var containerName = "images";
                var blobUri = await _blobService.UploadBlob(blobName, containerName, image);
                if (string.IsNullOrEmpty(blobUri))
                {
                    throw new FailToUploadException();
                }
                product.Images.Add(blobUri);
                uploadedBlobs.Add((blobName, containerName));
            }
            
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            foreach (var (blobName, containerName) in uploadedBlobs)
            {
                await _blobService.DeleteBlob(blobName, containerName);
            }

            throw new ApplicationException($"Failed to create product: {e.Message}", e);
        }
    }
}