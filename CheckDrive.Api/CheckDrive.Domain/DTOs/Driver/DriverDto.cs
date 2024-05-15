namespace CheckDrive.Domain.DTOs.Driver;
public record DriverDto(
    int Id,
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate)
{
    // Parameterless constructor required by AutoMapper
    public DriverDto() : this(default, default, default, default, default, default, default) { }
}
