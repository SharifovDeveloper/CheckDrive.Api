namespace CheckDrive.Domain.DTOs.Account;
public record AccountForCreateDto(
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate,
    int RoleId);