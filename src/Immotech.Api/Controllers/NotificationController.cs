using Application.Commands.Notification;
using Application.Queries.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Immotech.Api.Controllers;

[Route("notification")]
public class NotificationController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetNotificationByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPut("{id:int}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var command = new MarkNotificationAsReadCommand { Id = id };
        await Mediator.Send(command);
        return NoContent();
    }
} 