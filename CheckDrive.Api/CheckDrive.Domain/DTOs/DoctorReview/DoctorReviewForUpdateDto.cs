namespace CheckDrive.Domain.DTOs.DoctorReview;
public record DoctorReviewForUpdateDto(
    int Id,
    bool IsHealthy,
    string? Comments,
    DateTime Date,
    int DriverId,
    int DoctorId);