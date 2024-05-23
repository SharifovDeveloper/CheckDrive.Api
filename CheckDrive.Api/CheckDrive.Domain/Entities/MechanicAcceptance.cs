using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class MechanicAcceptance : EntityBase
    {
        public bool IsAccepted { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }
        public int MechanicId { get; set; }
        public Mechanic Mechanic { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}
