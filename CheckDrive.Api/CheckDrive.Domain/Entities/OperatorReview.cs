using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class OperatorReview : EntityBase
    {
        public bool IsGiven { get; set; }
        public double OilAmount { get; set; }
        public string? Comments { get; set; }
        public OilMarks OilMarks { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        public virtual ICollection<DispatcherReview> DispatcherReviews { get; set; }
    }
}
