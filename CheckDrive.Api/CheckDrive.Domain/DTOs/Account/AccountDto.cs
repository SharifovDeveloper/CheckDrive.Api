namespace CheckDrive.Domain.DTOs.Account;
public record AccountDto(
    int Id,
    string Login,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateTime Bithdate,
    int RoleId);