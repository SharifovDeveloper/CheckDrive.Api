namespace CheckDrive.Domain.ResourceParameters
{
    public class AccountResourceParameters : ResourceParametersBase
    {
        public int? RoleId { get; set; }
        public override string OrderBy { get; set; } = "firstname";
    }
}
