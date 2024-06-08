using System.Collections.Generic;

namespace CheckDrive.ApiContracts.Dashboard
{
    public class DashboardDto
    {
        public Summary Summary { get; set; }
        public IEnumerable<SpliteChartData> SplineCharts { get; set; }

        public DashboardDto(Summary summary, IEnumerable<SpliteChartData> spliteChartDatas)
        {
            Summary = summary;
            SplineCharts = spliteChartDatas;
        }
    }
    public class Summary
    {
        public int CarsCount { get; set; }
        public int DriversCount { get; set; }
        public double MonthlyFuelConsumption { get; set; }

        public Summary(int carscount, int driverscount, double monthslyFuelConsumption)
        {
            CarsCount = carscount;
            DriversCount = driverscount;
            MonthlyFuelConsumption = monthslyFuelConsumption;
        }
    }
    public class SpliteChartData
    {
        public string Month { get; set; }
        public decimal Ai80 { get; set; }
        public decimal Ai91 { get; set; }
        public decimal Ai92 { get; set; }
        public decimal Ai95 { get; set; }
    }
}
