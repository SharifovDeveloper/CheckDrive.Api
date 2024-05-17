namespace CheckDrive.Domain.DTOs.DispatcherReview;
public record DispatcherReviewDto(
    int Id,
    double FuelSpended,
    double DistanceCovered,
    DateTime Date,
    int DispatcherId,
    int OperatorId,
    int MechanicId,
    int DriverId);