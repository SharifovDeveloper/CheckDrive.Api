namespace CheckDrive.Domain.DTOs.Operator;
public record OperatorForCreateDto(
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Birthdate);