using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Delete;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, int>
{
    private readonly ProductContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteCategoryCommandHandler(ProductContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (category == null)
            {
                throw new CategoryNotFoundException(request.Id);
            }

            if (category.HasChild)
            {
                throw new InvalidOperationException("Cannot delete a category that has children.");
            }

            // Delete image from Blob Storage
            if (!string.IsNullOrEmpty(category.Image))
            {
                var blobName = new Uri(category.Image).Segments.Last();
                var containerName = "images";
                await _blobService.DeleteBlob(blobName, containerName);
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (category.ParentId.HasValue)
            {
                var parent = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.ParentId == category.ParentId.Value, cancellationToken);
                if (parent != null)
                {
                    parent.HasChild =
                        await _dbContext.Categories.AnyAsync(c => c.ParentId == parent.Id, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            await transaction.CommitAsync(cancellationToken);
            return category.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new ApplicationException($"Failed to delete category: {ex.Message}", ex);
        }
    }
}