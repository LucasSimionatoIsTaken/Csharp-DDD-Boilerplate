namespace Application.Seedwork;

public class BaseResponse<T> where T : class
{
    public string? Message { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    
    //TODO: Validation errors
}