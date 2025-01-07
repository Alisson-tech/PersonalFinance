using FinanceSimplify.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceSimplify.Test.IntegrationTesting.Context;

public class ContextFinanceTest : IDisposable
{
    private readonly DbContextOptions<ContextFinance> _options;

    public ContextFinanceTest()
    {
        _options = new DbContextOptionsBuilder<ContextFinance>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    public ContextFinance CreateContext()
    {
        return new ContextFinance(_options);
    }

    public void Dispose()
    {

    }
}
