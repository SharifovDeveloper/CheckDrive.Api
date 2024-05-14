namespace CheckDrive.Domain.ResourceParameters
{
    public class DoctorResourceParameters : ResourceParametersBase
    {
        public int? AccountId { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
