namespace CheckDriver.Domain.Entities
{
    public class Operator
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DispatcherReview> DispetcherReviews { get; set; }
        public  virtual ICollection<OperatorReview> OperatorReviews { get; set; }
    }
}
