namespace CheckDrive.Domain.DTOs.Doctor;
public record DoctorDto(
    int Id,
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate)
{

    // Parameterless constructor required by AutoMapper
    public DoctorDto() : this(default, default, default, default, default, default, default) { }
}
