namespace CheckDriver.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DoctorReview> DoctorReviews { get; set; }
    }
}
