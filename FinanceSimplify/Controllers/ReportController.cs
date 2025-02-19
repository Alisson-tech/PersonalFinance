﻿using FinanceSimplify.Exceptions;
using FinanceSimplify.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ReportController : Controller
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("Category/General")]
    public async Task<ActionResult<CategoryGeneralReportDto>> GetCategoryGeneralReport([FromQuery] CategoryFilterReport filter)
    {
        try
        {
            return Ok(await _reportService.GetCategoryGeneralReport(filter));
        }
        catch (FinanceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }
    }

    [HttpGet("Category/Percentage")]
    public async Task<ActionResult<CategoryPercentageReportDto>> GetCategoryPercentageValueGeneralReport([FromQuery] CategoryFilterReport filter)
    {
        try
        {
            return Ok(await _reportService.GetCategoryPercentage(filter));
        }
        catch (FinanceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }
    }
}
