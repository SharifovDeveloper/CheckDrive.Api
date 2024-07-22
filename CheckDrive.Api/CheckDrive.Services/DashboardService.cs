using CheckDrive.ApiContracts.Dashboard;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;
public class DashboardService : IDashboardService
{
    private readonly CheckDriveDbContext _context;
    public DashboardService(CheckDriveDbContext context)
    {
        _context = context;
    }
    public async Task<DashboardDto> GetDashboard()
    {
        var summary = await GetSummaryAsync();
        var splineChartData = await GetSpliteChartDataAsync();
        var employeesCountByRole = await GetEmployeesCountAsync();

        return new DashboardDto(summary, splineChartData, employeesCountByRole);
    }
    private async Task<IEnumerable<EmployeesCountByRole>> GetEmployeesCountAsync()
    {
        return await _context.Accounts
            .GroupBy(a => a.Role.Name)
            .Select(g => new EmployeesCountByRole
            {
                RoleName = g.Key,
                CountOfEmployees = g.Count()
            })
            .ToListAsync();
    }

    private async Task<IEnumerable<SpliteChartData>> GetSpliteChartDataAsync()
    {

        var reviews = await _context.OperatorReviews
                                   .GroupBy(r => new { r.Date.Year, r.Date.Month })
                                   .Select(g => new
                                   {
                                       Year = g.Key.Year,
                                       Month = g.Key.Month,
                                       Ai80 = g.Where(x => x.OilMarks == OilMarks.A80).Sum(x => (decimal?)x.OilAmount) ?? 0,
                                       Ai91 = g.Where(x => x.OilMarks == OilMarks.A91).Sum(x => (decimal?)x.OilAmount) ?? 0,
                                       Ai92 = g.Where(x => x.OilMarks == OilMarks.A92).Sum(x => (decimal?)x.OilAmount) ?? 0,
                                       Ai95 = g.Where(x => x.OilMarks == OilMarks.A95).Sum(x => (decimal?)x.OilAmount) ?? 0
                                   })
                                   .ToListAsync();

        var result = reviews
                     .Select(r => new SpliteChartData
                     {
                         Month = new DateTime(r.Year, r.Month, 1).ToString("MMMM"),
                         Ai80 = r.Ai80,
                         Ai91 = r.Ai91,
                         Ai92 = r.Ai92,
                         Ai95 = r.Ai95
                     })
                     .ToList();

        return result;
    }
    private async Task<Summary> GetSummaryAsync()
    {
        var carsCount = await _context.Cars.CountAsync();
        var driversCount = await _context.Drivers.CountAsync();

        var lastMonth = DateTime.Now.AddMonths(-1);
        var startOfLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
        var startOfThisMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        double totalOilAmount = await _context.OperatorReviews
                                    .Where(or => or.Date >= startOfLastMonth && or.Date < startOfThisMonth)
                                    .SumAsync(or => or.OilAmount);

        return new Summary(carsCount, driversCount, totalOilAmount);
    }
}

