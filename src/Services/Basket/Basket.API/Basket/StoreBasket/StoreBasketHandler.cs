﻿using Basket.API.Data;

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

        public StoreBasketCommandHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.Cart;

            await _repository.StoreBasket(cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}
