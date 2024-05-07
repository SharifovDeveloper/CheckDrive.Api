namespace CheckDriver.Domain.Entities
{
    public class MechanicAcceptance
    {
        public int Id { get; set; }

        public bool IsAccepted { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }

        public int MechanicHandoverId { get; set; }
        public MechanicHandover MechanicHandover { get; set; }
    }
}
