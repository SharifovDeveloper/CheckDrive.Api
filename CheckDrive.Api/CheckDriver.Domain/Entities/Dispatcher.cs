namespace CheckDriver.Domain.Entities
{
    public class Dispatcher
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DispatcherReview> DispetcherReviews { get; set; }
    }
}
