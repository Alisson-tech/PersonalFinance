namespace FinanceSimplify.Data;

public abstract class BaseEntity
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
}

