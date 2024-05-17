namespace CheckDrive.Domain.DTOs.Driver;
public record DriverForCreateDto(
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Birthdate);