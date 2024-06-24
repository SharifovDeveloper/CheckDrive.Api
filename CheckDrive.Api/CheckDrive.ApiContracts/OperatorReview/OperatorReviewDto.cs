using System;

namespace CheckDrive.ApiContracts.OperatorReview
{
    public class OperatorReviewDto
    {
        public int Id { get; set; }
        public bool IsGiven { get; set; }
        public double OilAmount { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public OilMarksForDto OilMarks { get; set; }
        public DateTime? Date { get; set; }

        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; }
        public string CarNumber { get; set; }
        public string CarOilCapacity { get; set; }
        public string CarOilRemainig{ get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int AccountDriverId { get; set; }
    }
}
