using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventoryReservation.Application.Commands.CreateItem;
using InventoryReservation.Application.Commands.ReserveItem;
using InventoryReservation.Application.Commands.AdjustQuantity;
using InventoryReservation.Application.Commands.CancelReservation;
using InventoryReservation.Application.Interfaces;

namespace InventoryReservation.Api.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InventoryController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromServices] IInventoryRepository repo, Guid id)
        {
            var item = await repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(new {
                item.Id,
                item.Sku,
                item.Name,
                item.QuantityOnHand,
                item.ReservedQuantity,
                available = item.AvailableQuantity()
            });
        }

        [HttpGet]
        public async Task<IActionResult> List([FromServices] IInventoryRepository repo)
        {
            var items = await repo.ListAsync();
            return Ok(items);
        }

        [HttpPost("{id:guid}/reserve")]
        public async Task<IActionResult> Reserve(Guid id, [FromBody] ReserveDto dto)
        {
            await _mediator.Send(new ReserveItemCommand(id, dto.Quantity));
            return NoContent();
        }

        [HttpPost("{id:guid}/adjust")]
        public async Task<IActionResult> Adjust(Guid id, [FromBody] AdjustDto dto)
        {
            await _mediator.Send(new AdjustQuantityCommand(id, dto.Delta));
            return NoContent();
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelDto dto)
        {
            await _mediator.Send(new CancelReservationCommand(id, dto.Quantity));
            return NoContent();
        }

        public record ReserveDto(int Quantity);
        public record AdjustDto(int Delta);
        public record CancelDto(int Quantity);
    }
}
