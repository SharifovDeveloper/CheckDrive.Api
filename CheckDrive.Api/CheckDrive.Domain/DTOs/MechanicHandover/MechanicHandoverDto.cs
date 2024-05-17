﻿using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicHandover;
public record MechanicHandoverDto(
    int Id,
    bool IsHanded,
    string? Comments,
    Status Status,
    DateTime Date,
    int MechanicId,
    int CarId,
    int DriverId);
