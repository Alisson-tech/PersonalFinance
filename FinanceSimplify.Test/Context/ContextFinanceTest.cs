using FinanceSimplify.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceSimplify.Test.Context;

public class ContextFinanceTest
{
    public ContextFinance CreateContext()
    {
        var options = new DbContextOptionsBuilder<ContextFinance>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ContextFinance(options);


        return context;
    }
}
