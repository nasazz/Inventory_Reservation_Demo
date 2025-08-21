using FluentValidation;

namespace InventoryReservation.Application.Commands.AdjustQuantity
{
    public class AdjustQuantityCommandValidator : AbstractValidator<AdjustQuantityCommand>
    {
        public AdjustQuantityCommandValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Delta).NotEqual(0).WithMessage("Delta must not be zero");
        }
    }
}
