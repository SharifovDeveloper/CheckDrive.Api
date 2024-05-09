using CheckDrive.Domain.DTOs.Car;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Mechanic;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicHandover
{
    public class MechanicHandoverForCreateDto
    {
        public bool IsHanded { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public MechanicDto MechanicDto { get; set; }
        public CarDto CarDto { get; set; }
        public DriverDto DriverDto { get; set; }
    }
}
