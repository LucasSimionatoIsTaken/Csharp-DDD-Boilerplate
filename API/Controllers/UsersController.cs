using Application.SeedWork.Responses;
using Application.User;
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
    [ProducesResponseType(typeof(PaginatedSuccessResponse<ListAll.Response>), StatusCodes.Status200OK)]
    public async Task<BaseResponse<ListAll.Response>> List()
        => await _mediator.Send(new ListAll.Request());
    
    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="request">User data</param>
    /// <returns>A newly created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">One or more parameters are missing or wrong</response>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessResponse<Create.Response>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse<>), StatusCodes.Status400BadRequest)]
    public async Task<BaseResponse<Create.Response>> Create([FromBody] Create.Request request)
        => await _mediator.Send(request);
}