namespace CheckDrive.Domain.DTOs.Mechanic;
public record MechanicDto(
    int Id,
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate)
{
    // Parameterless constructor required by AutoMapper
    public MechanicDto() : this(default, default, default, default, default, default, default) { }
}
