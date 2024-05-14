using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class Doctor : EntityBase
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DoctorReview> DoctorReviews { get; set; }
    }
}
