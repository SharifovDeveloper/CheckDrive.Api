using System;

namespace CheckDrive.ApiContracts.MechanicAcceptance
{
    public class MechanicAcceptanceDto
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public string Comments { get; set; } = "";
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }
        public int MechanicHandoverId { get; set; }
        public string MechanicHandoverName { get; set; }
    }
}
