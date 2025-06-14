using Application.Commands.Photo;
using Application.Queries.Photo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Immotech.Api.Controllers;

[Route("photo")]
public class PhotoController : BaseApiController
{
    [HttpGet]
    [AllowAnonymous] // Allow all users to access this endpoint
    public async Task<IActionResult> GetAll([FromQuery] GetAllPhotosQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Allow all users to access this endpoint
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetPhotoByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePhotoCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePhotoCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeletePhotoCommand { Id = id });
        return NoContent();
    }
} 