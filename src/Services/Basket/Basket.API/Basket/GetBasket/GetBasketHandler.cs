
using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName): IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);
    internal class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly IBasketRepository _repository;

        public GetBasketQueryHandler(IBasketRepository basketRepository)
        {
            _repository = basketRepository;
        }

        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _repository.GetBasket(request.UserName);

            return new GetBasketResult(basket);
        }
    }
}
