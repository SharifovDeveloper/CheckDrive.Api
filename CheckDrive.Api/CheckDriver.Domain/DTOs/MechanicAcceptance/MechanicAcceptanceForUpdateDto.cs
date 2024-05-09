using CheckDrive.Domain.DTOs.MechanicHandover;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.DTOs.MechanicAcceptance
{
    public class MechanicAcceptanceForUpdateDto
    {
        public bool IsAccepted { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public double Distance { get; set; }

        public MechanicHandoverDto MechanicHandoverDto { get; set; }
    }
}
