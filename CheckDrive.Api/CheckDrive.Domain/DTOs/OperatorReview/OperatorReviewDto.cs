using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.DTOs.OperatorReview;
public record OperatorReviewDto(
    int Id,
    double OilAmount,
    string? Comments,
    Status Status,
    DateTime Date,
    int OperatorId,
    int DriverId);