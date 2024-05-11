using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicHandover
{
    public class MechanicHandoverForUpdateDto
    {
        public int Id { get; set; }
        public bool IsHanded { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int MechanicId { get; set; }
        public int CarId { get; set; }
        public int DriverId { get; set; }
    }
}
