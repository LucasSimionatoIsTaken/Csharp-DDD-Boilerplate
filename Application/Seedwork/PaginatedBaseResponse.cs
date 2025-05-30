namespace Application.Seedwork;

public class PaginatedBaseResponse<T> where T : class
{
    public List<T>? Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}