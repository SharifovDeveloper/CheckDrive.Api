namespace CheckDrive.Domain.DTOs.DispatcherReview;
public record DispatcherReviewForUpdateDto(
    int Id,
    double FuelSpended,
    double DistanceCovered,
    DateTime Date,
    int DispatcherId,
    int OperatorId,
    int MechanicId,
    int DriverId);