using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.OperatorReview;
public record OperatorReviewForCreateDto(
    double OilAmount,
    string? Comments,
    Status Status,
    DateTime Date,
    int OperatorId,
    int DriverId);