namespace CheckDriver.Domain.Entities
{
    public class Mechanic
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public ICollection<DispatcherReview>? DispetcherReviews { get; set; }
        public ICollection<MechanicHandover>? MechanicHandovers { get; set; }
    }
}
