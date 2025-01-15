using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CategoryGeneralReportDto> GetCategoryGeneralReport(CategoryFilterReport filter)
    {
        var valueCategory = await _transactionRepository.GetIqueryble()
            .Where(t => (t.DateCreated >= filter.DateStart) &&
                (t.DateCreated <= filter.DateFinish) &&
                (filter.AccountId == null || t.AccountId == filter.AccountId) &&
                (filter.TransactionType == null || t.Type == filter.TransactionType))
            .GroupBy(t => t.Category)
            .Select(group => new CategoryReport
            {
                Category = group.Key,
                Value = group.Sum(t => t.Type == TransactionType.Income ? -t.Value : t.Value)
            }).ToListAsync();

        return new CategoryGeneralReportDto
        {
            CategoryReports = valueCategory
        };
    }

    public Task<List<CategoryPercentageReportDto>> GetCategoryPercentage(CategoryFilterReport filter)
    {
        throw new NotImplementedException();
    }
}
