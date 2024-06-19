using System;

namespace CheckDrive.ApiContracts.MechanicHandover
{
    public class MechanicHandoverDto
    {
        public int Id { get; set; }
        public bool IsHanded { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime? Date { get; set; }
        public double Distance { get; set; }
        public int MechanicId { get; set; }
        public string MechanicName { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int AccountDriverId { get; set; }
    }
}
