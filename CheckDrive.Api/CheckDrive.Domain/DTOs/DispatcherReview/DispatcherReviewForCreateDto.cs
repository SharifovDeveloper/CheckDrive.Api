namespace CheckDrive.Domain.DTOs.DispatcherReview;
public record DispatcherReviewForCreateDto(
    double FuelSpended,
    double DistanceCovered,
    DateTime Date,
    int DispatcherId,
    int OperatorId,
    int MechanicId,
    int DriverId);