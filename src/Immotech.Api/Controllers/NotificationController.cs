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
        await Mediator.Send(command);
        return Accepted();
    }


    [HttpPut("{id:int}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var command = new MarkNotificationAsReadCommand { Id = id };
        await Mediator.Send(command);
        return NoContent();
    }
} 