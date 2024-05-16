namespace CheckDrive.Domain.DTOs.Mechanic;
public record MechanicForCreateDto(
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Birthdate);