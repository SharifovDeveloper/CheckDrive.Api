namespace CheckDrive.Domain.DTOs.Doctor;
public record DoctorForCreateDto(
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Birthdate);