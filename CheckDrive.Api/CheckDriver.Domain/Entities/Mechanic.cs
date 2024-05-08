using CheckDriver.Domain.Common;

namespace CheckDriver.Domain.Entities
{
    public class Mechanic : EntityBase
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DispatcherReview> DispetcherReviews { get; set; }
        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
    }
}
