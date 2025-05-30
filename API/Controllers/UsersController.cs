using Application.User.ListAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// List all users
    /// </summary>
    /// <returns>A users list</returns>
    /// <response code="200">A user list</response>
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await _mediator.Send(new Request()));
    }
    
    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="request">User data</param>
    /// <returns>A newly created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">One or more parameters are missing or wrong</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Application.User.Create.Request request)
    {
        return Ok(await _mediator.Send(request));
    }
}