﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Contracts;
using RookiEcom.Application.Exceptions;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Update;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly ProductContext _productContext;
    private readonly IBlobService _blobService;
    private readonly IValidator<UpdateProductCommand> _validator;
    public UpdateProductCommandHandler(
        ProductContext productContext,
        IBlobService blobService,
        IValidator<UpdateProductCommand> validator)
    {
        _productContext = productContext;
        _blobService = blobService;
        _validator = validator;
    }
    
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        await using var transaction = await _productContext.Database.BeginTransactionAsync(cancellationToken);
        var uploadedBlobs = new List<(string BlobName, string ContainerName)>();
        var oldImageUris = new List<string>();

        try
        {
            var product = await _productContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            
            oldImageUris.AddRange(product.Images);
            product.Images.Clear();

            if (request.OldImages != null)
            {
                product.Images.AddRange(request.OldImages);
            }

            if (request.NewImages.Any())
            {
                foreach (var image in request.NewImages)
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
            }

            if (product.SKU != request.SKU && await _productContext.Products.AnyAsync(p => p.SKU == request.SKU, cancellationToken))
            {
                throw new ProductSkuExistedException(request.SKU);
            }
            
            product.SKU = request.SKU;
            product.CategoryId = request.CategoryId;
            product.Name = request.Name;
            product.Description = request.Description;
            product.MarketPrice = request.MarketPrice;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.IsFeature = request.IsFeature;
            product.Status = request.Status;
            product.ProductAttributes = request.ProductAttributes;
            product.ProductOption = request.ProductOption;
            product.UpdatedDateTime = DateTime.UtcNow;

            await _productContext.SaveChangesAsync(cancellationToken);

            foreach (var oldImageUri in oldImageUris)
            {
                // Only delete old images that are not in the new list
                if (!product.Images.Contains(oldImageUri))
                {
                    var blobName = new Uri(oldImageUri).Segments.Last();
                    var containerName = "images";
                    await _blobService.DeleteBlob(blobName, containerName);
                }
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var (blobName, containerName) in uploadedBlobs)
            {
                await _blobService.DeleteBlob(blobName, containerName);
            }

            throw;
        }
    }
}