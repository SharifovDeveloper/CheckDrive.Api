using CheckDrive.Domain.Common;
namespace CheckDrive.Domain.Entities
{
    public class DispatcherReview : EntityBase
    {
        public double FuelSpended { get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public int DispatcherId { get; set; }
        public Dispatcher Dispatcher { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
        public int MechanicId { get; set; }
        public Mechanic Mechanic { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int OperatorReviewId { get; set; }
        public OperatorReview OperatorReview { get; set; }
        public int MechanicHandoverId { get; set; }
        public MechanicHandover MechanicHandover { get; set; }
        public int MechanicAcceptanceId { get; set; }
        public MechanicAcceptance MechanicAcceptance { get; set; }
    }
}
