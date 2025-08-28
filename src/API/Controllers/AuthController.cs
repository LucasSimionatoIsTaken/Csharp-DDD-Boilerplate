using Application.Services.Auth;
using Application.SeedWork.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.SeedWork.Filters.Swagger;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    [HttpPost]
    [ProducesResponseType(typeof(DataResponse<Login.Response>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NoDataResponse<>), StatusCodes.Status401Unauthorized)]
    public async Task<BaseResponse<Login.Response>> Login([FromBody] Login.Request request)
        => await _mediator.Send(request);
}