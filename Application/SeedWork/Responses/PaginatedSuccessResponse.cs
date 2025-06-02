namespace Application.SeedWork.Responses;

public class PaginatedSuccessResponse<T> : BaseResponse<T> where T : class
{
    //TODO: Complete pagination, with Total, Page and PageSize
    public PaginatedSuccessResponse(int statusCode, List<T> data)
    {
        StatusCode = statusCode;
        Data = data;
    }

    public PaginatedSuccessResponse(List<T> data)
    {
        StatusCode = 200;
        Data = data;
    }

    public List<T> Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}