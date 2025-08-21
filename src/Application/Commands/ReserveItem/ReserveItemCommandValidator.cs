using FluentValidation;

namespace InventoryReservation.Application.Commands.ReserveItem
{
    public class ReserveItemCommandValidator : AbstractValidator<ReserveItemCommand>
    {
        public ReserveItemCommandValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
