using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InventoryReservation.Application.Commands.CreateItem;
using InventoryReservation.Application.Interfaces; // <<-- add this

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
                item.ReservedQuantity
            });
        }

        [HttpGet]
        public async Task<IActionResult> List([FromServices] IInventoryRepository repo)
        {
            var items = await repo.ListAsync();
            return Ok(items);
        }
    }
}
