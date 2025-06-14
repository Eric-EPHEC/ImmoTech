using Application.Commands.ProfessionalUser;
using Application.Queries.ProfessionalUser;
using Microsoft.AspNetCore.Mvc;

namespace Immotech.Api.Controllers;

[Route("professional-user")]
public class ProfessionalUserController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProfessionalUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetProfessionalUserByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfessionalUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProfessionalUserCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteProfessionalUserCommand { Id = id });
        return NoContent();
    }
} 