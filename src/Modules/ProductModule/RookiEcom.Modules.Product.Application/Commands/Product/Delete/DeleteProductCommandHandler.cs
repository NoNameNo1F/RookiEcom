using FluentValidation;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Delete;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly ProductContext _productContext;
    private readonly IBlobService _blobService;
    private readonly IValidator<DeleteProductCommand> _validator;


    public DeleteProductCommandHandler(
        ProductContext productContext,
        IBlobService blobService,
        IValidator<DeleteProductCommand> validator)
    {
        _productContext = productContext;
        _blobService = blobService;
        _validator = validator;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        await using var transaction = await _productContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var product = await _productContext.Products.FindAsync(request.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            
            foreach (var imageUri in product.Images)
            {
                var blobName = new Uri(imageUri).Segments.Last();
                var containerName = "images";
                await _blobService.DeleteBlob(blobName, containerName);
            }

            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw new ApplicationException($"Failed to delete product: {ex.Message}", ex);
        }
    }
}