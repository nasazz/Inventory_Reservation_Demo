using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using InventoryReservation.Application.Commands.ReserveItem;
using InventoryReservation.Application.Interfaces;
using InventoryReservation.Domain.Entities;

namespace UnitTests
{
    public class ReserveItemHandlerTests
    {
        [Fact]
        public async Task Handler_Should_Call_SaveChanges_When_Item_Found_And_Reserved()
        {
            // arrange
            var itemId = Guid.NewGuid();
            var item = new InventoryItem(itemId, "SKU-1", "Test", 10);

            var repoMock = new Mock<IInventoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

            var handler = new ReserveItemCommandHandler(repoMock.Object);

            var cmd = new ReserveItemCommand(itemId, 3);

            // act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // assert
            repoMock.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            item.ReservedQuantity.Should().Be(3);
        }

        [Fact]
        public async Task Handler_Should_Throw_When_Item_Not_Found()
        {
            var repoMock = new Mock<IInventoryRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((InventoryItem?)null);

            var handler = new ReserveItemCommandHandler(repoMock.Object);

            var cmd = new ReserveItemCommand(Guid.NewGuid(), 1);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
        }
    }
}
