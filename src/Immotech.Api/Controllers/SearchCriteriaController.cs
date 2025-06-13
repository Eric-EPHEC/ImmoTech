using Application.Commands.SearchCriteria;
using Application.Queries.SearchCriteria;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Immotech.Api.Controllers;

[Route("search-criteria")]
public class SearchCriteriaController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSearchCriteriaCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetSearchCriteriaByIdQuery { Id = id });
        return result != null ? Ok(result) : NotFound();
    }
}