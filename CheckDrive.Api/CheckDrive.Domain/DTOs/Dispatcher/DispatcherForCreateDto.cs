namespace CheckDrive.Domain.DTOs.Dispatcher;
public record DispatcherForCreateDto(
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Birthdate);