using System;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.ApiContracts.OperatorReview
{
    public class OperatorReviewForCreateDto
    {
        public bool IsGiven { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Quyiladigan yoqilg'i manfiy bo'lishi mumkin emas")]
        public double OilAmount { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public OilMarksForDto OilMarks { get; set; }
        public DateTime Date { get; set; }

        public int OperatorId { get; set; }
        public int DriverId { get; set; }
        public int CarId { get; set; }
    }
}
