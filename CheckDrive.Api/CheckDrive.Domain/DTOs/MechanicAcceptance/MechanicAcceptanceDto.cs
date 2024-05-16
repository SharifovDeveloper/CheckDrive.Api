using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicAcceptance;
public record MechanicAcceptanceDto(
    int Id,
    bool IsAccepted,
    string? Comments,
    Status Status,
    DateTime Date,
    double Distance,
    int MechanicHandoverId);