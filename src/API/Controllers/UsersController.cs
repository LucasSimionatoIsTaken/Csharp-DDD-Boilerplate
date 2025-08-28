using Application.SeedWork.Responses;
using Application.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.SeedWork.Filters.Swagger;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
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
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ListAll.Response>), StatusCodes.Status200OK)]
    public async Task<BaseResponse<ListAll.Response>> List([FromQuery] ListAll.RequestQuery query)
        => await _mediator.Send(query);

    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="request">User data</param>
    /// <returns>A newly created user</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="422">One or more parameters are missing or wrong</response>
    [HttpPost]
    [ProducesResponseType(typeof(DataResponse<Create.Response>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorListResponse<>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<BaseResponse<Create.Response>> Create([FromBody] Create.Request request)
        => await _mediator.Send(request);

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DataResponse<Update.Response>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NoDataResponse<>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorListResponse<>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<BaseResponse<Update.Response>> Update([FromRoute] Guid id, [FromBody] Update.Request request)
    {
        request.SetId(id);
        return await _mediator.Send(request);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NoDataResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NoDataResponse<>), StatusCodes.Status404NotFound)]
    public async Task<BaseResponse<object>> Delete([FromRoute] Guid id)
    {
        var request = new Delete.Request(id);
        return await _mediator.Send(request);
    }

}