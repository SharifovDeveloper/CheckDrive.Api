namespace CheckDriver.Domain.Entities
{
    public class Operator
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public ICollection<DispatcherReview>? DispetcherReviews { get; set; }
        public ICollection<OperatorReview>? OperatorReviews { get; set; }
    }
}
