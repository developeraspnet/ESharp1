using FluentValidation;
using NetTopologySuite.Algorithm;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
                    Guid Id,
                    string Name,
                    List<string> Category,
                    string Description,
                    string ImageFile,
                    decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
        
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
    }
}

internal class UpdateProductCommandHandler(
    IDocumentSession documentSession, ILogger<UpdateProductCommandHandler> logger)
    :ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductCommandHandler.Handle called with {command}", command);

        var product = await documentSession.LoadAsync<Product>(command.Id);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        
        documentSession.Update(product);
        await documentSession.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);

    }
}