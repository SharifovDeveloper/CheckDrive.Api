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

        public int MechanicHandoverId { get; set; }
        public MechanicHandover MechanicHandover { get; set; }
    }
}
