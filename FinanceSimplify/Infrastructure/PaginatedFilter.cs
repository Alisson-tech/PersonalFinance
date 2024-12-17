namespace FinanceSimplify.Infraestructure;


public class PaginatedFilter
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string OrderBy { get; set; } = "DateCreated"; 
    public bool OrderAsc { get; set; } = false;
}
