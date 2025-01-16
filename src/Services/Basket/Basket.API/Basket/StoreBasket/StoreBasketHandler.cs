﻿using Basket.API.Data;
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart): ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator: AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    internal class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _repository = repository;
            _discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);

            await _repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
}
