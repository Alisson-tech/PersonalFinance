using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;

namespace FinanceSimplify.Services.Report;

public class ReportService : IReportService
{
    private readonly IGenericRepository<Transactions> _transactionRepository;
    private readonly IMapper _mapper;

    public ReportService(IGenericRepository<Transactions> transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public Task<CategoryGeneralReportDto> GetCategoryGeneralReport(CategoryFilterReport filter)
    {
        throw new NotImplementedException();
    }

    public Task<List<CategoryPercentageReportDto>> GetCategoryPercentage(CategoryFilterReport filter)
    {
        throw new NotImplementedException();
    }
}
