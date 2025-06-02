namespace Application.SeedWork.Responses;

public class SuccessResponse<T> : BaseResponse<T> where T : class
{
    public SuccessResponse(int statusCode, string message, T? data) : base(message)
    {
        StatusCode = statusCode;
        Data = data;
    }

    public SuccessResponse(string message, T? data) : base(message)
    {
        StatusCode = 200;
        Data = data;
    }
    
    public T? Data { get; private set; }
}