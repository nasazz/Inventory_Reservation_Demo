using FluentValidation;

namespace InventoryReservation.Application.Commands.CreateItem
{
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.QuantityOnHand).GreaterThanOrEqualTo(0);
        }
    }
}
