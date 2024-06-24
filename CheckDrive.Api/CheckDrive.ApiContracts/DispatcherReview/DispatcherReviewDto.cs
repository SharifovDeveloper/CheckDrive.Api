using System;

namespace CheckDrive.ApiContracts.DispatcherReview
{
    public class DispatcherReviewDto
    {
        public int Id { get; set; }
        public double FuelSpended { get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public double InitialDistance { get; set; }
        public int MechanicHandoverId { get; set; }
        public double FinalDistance { get; set; }
        public int MechanicAcceptanceId { get; set; }
        public int DispatcherId { get; set; }
        public string DispatcherName { get; set; }
        public double PouredFuel { get; set; }
        public int OperatorReviewId { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public int MechanicId { get; set; }
        public string MechanicName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; }
        public double CarMeduimFuelConsumption { get; set; }
    }
}
