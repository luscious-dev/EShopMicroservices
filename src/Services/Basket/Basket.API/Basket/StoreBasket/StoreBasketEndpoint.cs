
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBaskerResponse(string UserName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var result = await sender.Send(new StoreBasketCommand(request.Cart));
                var response = result.Adapt<StoreBaskerResponse>();
                return Results.Created($"/basket/{response.UserName}", response);
            })
                .WithName("Store Basket")
                .WithSummary("Store Basket")
                .WithDescription("Store Basket")
                .Produces<StoreBaskerResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
