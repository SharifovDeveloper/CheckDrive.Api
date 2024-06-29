namespace CheckDrive.Domain.ResourceParameters
{
    public class DoctorReviewResourceParameters : ResourceParametersBase
    {
        public int? RoleId { get; set; }
        public int? DriverId { get; set; }
        public bool? IsHealthy { get; set; }
        public DateTime? Date { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
