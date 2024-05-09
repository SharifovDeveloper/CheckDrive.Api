namespace CheckDrive.Domain.ResourceParameters
{
    public class DoctorReviewResourceParameters : ResourceParametersBase
    {
        public bool? IsHealthy { get; set; }
        public DateTime? Date { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
