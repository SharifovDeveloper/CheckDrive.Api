namespace CheckDrive.Domain.DTOs.Dispatcher;
public record DispatcherDto(
    int Id,
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate)
{

    // Parameterless constructor required by AutoMapper
    public DispatcherDto() : this(default, default, default, default, default, default, default) { }
}
