namespace CheckDrive.Domain.DTOs.DoctorReview;
public record DoctorReviewDto(
    int Id,
    bool IsHealthy,
    string? Comments,
    DateTime Date);