using Application.Commands.Property;
using Application.Queries.Property;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Immotech.Api.Controllers;

[Route("property")]
public class PropertyController : BaseApiController
{
    [HttpGet]
    [AllowAnonymous] // Allow all users to access this endpoint
    public async Task<IActionResult> GetAll([FromQuery] GetAllPropertiesQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Allow all users to access this endpoint
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetPropertyByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropertyCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeletePropertyCommand { Id = id });
        return NoContent();
    }
} 