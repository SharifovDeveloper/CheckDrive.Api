namespace CheckDrive.Domain.ResourceParameters
{
    public class AccountResourceParameters : ResourceParametersBase
    {
        public DateTime? BirthDate { get; set; }
        public string? Login { get; set; }
        public string? Pasword { get; set; }
        public int? RoleId { get; set; }
        public override string OrderBy { get; set; } = "idDesc";
    }
}
