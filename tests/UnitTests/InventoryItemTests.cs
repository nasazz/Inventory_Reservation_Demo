using System;
using FluentAssertions;
using InventoryReservation.Domain.Entities;
using Xunit;

namespace UnitTests
{
    public class InventoryItemTests
    {
        [Fact]
        public void Reserve_Should_Increase_ReservedQuantity_When_Available()
        {
            // arrange
            var id = Guid.NewGuid();
            var item = new InventoryItem(id, "SKU-1", "Test Item", quantityOnHand: 5);

            // act
            item.Reserve(2);

            // assert
            item.ReservedQuantity.Should().Be(2);
            item.AvailableQuantity().Should().Be(3);
        }

        [Fact]
        public void Reserve_Should_Throw_When_Insufficient_Available()
        {
            var id = Guid.NewGuid();
            var item = new InventoryItem(id, "SKU-2", "Small Item", quantityOnHand: 2);

            Action act = () => item.Reserve(5);

            act.Should().Throw<InvalidOperationException>().WithMessage("*Not enough available*");
        }

        [Fact]
        public void CancelReservation_Should_Decrease_ReservedQuantity()
        {
            var id = Guid.NewGuid();
            var item = new InventoryItem(id, "SKU-3", "Cancel Test", quantityOnHand: 5);
            item.Reserve(3);

            item.CancelReservation(2);

            item.ReservedQuantity.Should().Be(1);
            item.AvailableQuantity().Should().Be(4);
        }
    }
}
