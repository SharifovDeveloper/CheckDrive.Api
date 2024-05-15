using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicAcceptance;
public record MechanicAcceptanceForCreateDto(
    bool IsAccepted,
    string? Comments,
    Status Status,
    DateTime Date,
    double Distance,
    int MechanicHandoverId);