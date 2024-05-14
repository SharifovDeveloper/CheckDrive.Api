using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Operator;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.OperatorReview
{
    public class OperatorReviewDto
    {
        public int Id { get; set; }
        public double OilAmount { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int OperatorId { get; set; }
        public int DriverId { get; set; }
    }
}
