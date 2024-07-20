using System;

namespace CheckDrive.DTOs.OperatorReview
{
    public class OperatorReviewForUpdateDto
    {
        public int Id { get; set; }
        public double OilAmount { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }

        public int OperatorId { get; set; }
        public int DriverId { get; set; }
    }
}
