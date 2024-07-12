namespace CheckDrive.Domain.ResourceParameters
{
    public class DriverResourceParameters : ResourceParametersBase
    {
        public int? RoleId { get; set; }
        public int? AccountId { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
