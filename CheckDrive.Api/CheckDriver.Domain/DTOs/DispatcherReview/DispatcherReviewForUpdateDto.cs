using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Mechanic;
using CheckDrive.Domain.DTOs.Operator;

namespace CheckDrive.Domain.DTOs.DispatcherReview
{
    public class DispatcherReviewForUpdateDto
    {
        public double FuelSpended { get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public DispatcherDto DispatcherDto { get; set; }
        public OperatorDto OperatorDto { get; set; }
        public MechanicDto MechanicDto { get; set; }
        public DriverDto DriverDto { get; set; }
    }
}
