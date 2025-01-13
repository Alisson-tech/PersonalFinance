using FinanceSimplify.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : Controller
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<ActionResult<CategoryGeneralReportDto>> GetCategoryGeneralReport([FromQuery] CategoryFilterReport filter)
    {
        throw new NotImplementedException();
    }
}
