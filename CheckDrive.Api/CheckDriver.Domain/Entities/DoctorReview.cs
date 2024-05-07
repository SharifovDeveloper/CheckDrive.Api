namespace CheckDriver.Domain.Entities
{
    public class DoctorReview
    {
        public int Id { get; set; }
        public bool IsHealthy { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
