using FluentValidation.Results;
using Mapster;

namespace Application.SeedWork.Responses;

public class ErrorResponse<T> : BaseResponse<T> where T : class
{
    public ErrorResponse(string message, List<ValidationFailure> errors) : base(message)
    {
        StatusCode = 400;
        Errors = errors.Adapt<List<ErrorItem>>();
    }

    public ErrorResponse(int statusCode, string message, List<ValidationFailure> errors) : base(message)
    {
        StatusCode = statusCode;
        Errors = errors.Adapt<List<ErrorItem>>();
    }
    
    public List<ErrorItem> Errors { get; private set; }
}