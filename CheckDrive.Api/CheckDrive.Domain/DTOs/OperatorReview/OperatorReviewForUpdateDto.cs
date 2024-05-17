using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.OperatorReview;
public record OperatorReviewForUpdateDto(
    int Id,
    double OilAmount,
    string? Comments,
    Status Status,
    DateTime Date,
    int OperatorId,
    int DriverId);