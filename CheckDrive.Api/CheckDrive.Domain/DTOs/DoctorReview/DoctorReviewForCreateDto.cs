namespace CheckDrive.Domain.DTOs.DoctorReview;
public record DoctorReviewForCreateDto(
    bool IsHealthy,
    string? Comments,
    DateTime Date,
    int DriverId,
    int DoctorId);

