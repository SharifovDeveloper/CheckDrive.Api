using System;

namespace CheckDrive.ApiContracts.MechanicAcceptance
{
    public class MechanicAcceptanceForUpdateDto
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }

        public int MechanicId { get; set; }
        public int CarId { get; set; }
        public int DriverId { get; set; }
    }
}
