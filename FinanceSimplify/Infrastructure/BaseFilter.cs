namespace FinanceSimplify.Infrastructure;

public class BaseFilter
{
    public DateTime DateStart { get; set; } = DateTime.Now.AddDays(-30);
    public DateTime DateFinish { get; set; } = DateTime.Now.AddDays(1);
}
