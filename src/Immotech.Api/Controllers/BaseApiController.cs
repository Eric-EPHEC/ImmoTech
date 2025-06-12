using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Immotech.Api.Controllers;

[ApiController]

public class BaseApiController : ControllerBase
{
   private IMediator? _mediator;

   protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

}
