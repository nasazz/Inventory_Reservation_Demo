using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventoryReservation.Application.Commands.CreateItem;
using InventoryReservation.Application.Commands.ReserveItem;
using InventoryReservation.Application.Commands.AdjustQuantity;
using InventoryReservation.Application.Commands.CancelReservation;
using InventoryReservation.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryReservation.Api.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IMediator mediator, ILogger<InventoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemCommand cmd)
        {
            try
            {
                var id = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Get), new { id }, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item");
                return StatusCode(500, "An error occurred while creating the item");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromServices] IInventoryRepository repo, Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the item");
            }
        }

        [HttpGet]
        public async Task<IActionResult> List([FromServices] IInventoryRepository repo)
        {
            try
            {
                var items = await repo.ListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing items");
                return StatusCode(500, "An error occurred while retrieving items");
            }
        }

        [HttpPost("{id:guid}/reserve")]
        public async Task<IActionResult> Reserve(Guid id, [FromBody] ReserveDto dto)
        {
            try
            {
                await _mediator.Send(new ReserveItemCommand(id, dto.Quantity));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reserving item {Id}", id);
                return StatusCode(500, "An error occurred while reserving the item");
            }
        }

        [HttpPost("{id:guid}/adjust")]
        public async Task<IActionResult> Adjust(Guid id, [FromBody] AdjustDto dto)
        {
            try
            {
                await _mediator.Send(new AdjustQuantityCommand(id, dto.Delta));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjusting quantity for item {Id}", id);
                return StatusCode(500, "An error occurred while adjusting the quantity");
            }
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelDto dto)
        {
            try
            {
                await _mediator.Send(new CancelReservationCommand(id, dto.Quantity));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling reservation for item {Id}", id);
                return StatusCode(500, "An error occurred while canceling the reservation");
            }
        }

        public record ReserveDto(int Quantity);
        public record AdjustDto(int Delta);
        public record CancelDto(int Quantity);
    }
}