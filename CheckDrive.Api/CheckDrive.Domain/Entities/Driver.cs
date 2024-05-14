using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class Driver : EntityBase
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DispatcherReview> DispetcherReviews { get; set; }
        public virtual ICollection<DoctorReview> DoctorReviews { get; set; }
        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
        public virtual ICollection<OperatorReview> OperatorReviews { get; set; }
    }
}
