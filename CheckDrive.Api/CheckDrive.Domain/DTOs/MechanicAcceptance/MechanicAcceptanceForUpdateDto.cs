using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicAcceptance;
public record MechanicAcceptanceForUpdateDto(
    int Id,
    bool IsAccepted,
    string? Comments,
    Status Status,
    DateTime Date,
    double Distance,
    int MechanicHandoverId);
