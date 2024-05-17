namespace CheckDrive.Domain.DTOs.Operator;
public record OperatorDto(
    int Id,
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate)
{
    // Parameterless constructor required by AutoMapper
    public OperatorDto() : this(default, default, default, default, default, default, default) { }
}
