using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Mechanic;
using CheckDrive.Domain.DTOs.Operator;

namespace CheckDrive.Domain.DTOs.DispatcherReview
{
    public class DispatcherReviewDto
    {
        public int Id { get; set; }
        public double FuelSpended { get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public int DispatcherId { get; set; }
        public int OperatorId { get; set; }
        public int MechanicId { get; set; }
        public int DriverId { get; set; }
        public DispatcherDto Dispatcher { get; set; }
        public OperatorDto Operator { get; set; }
        public MechanicDto Mechanic { get; set; }
        public DriverDto Driver { get; set; }
    }
}
