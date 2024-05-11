using CheckDrive.Domain.DTOs.Car;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.Mechanic;
using CheckDrive.Domain.DTOs.MechanicAcceptance;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicHandover
{
    public class MechanicHandoverDto
    {
        public int Id { get; set; }
        public bool IsHanded { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public MechanicDto Mechanic { get; set; }
        public CarDto Car { get; set; }
        public DriverDto Driver { get; set; }
    }
}
