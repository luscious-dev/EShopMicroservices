
namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id)
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
                .WithName("Delete Product")
                .WithSummary("Delete Product")
                .WithDescription("Delete Product")
                .ProducesProblem(StatusCodes.Status404NotFound)
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK);
        }
    }
}
