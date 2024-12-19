using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Create product entity from command
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Category = request.Category
            };

            // Save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // Return result
            return new CreateProductResult(product.Id);
        }
    }
}
