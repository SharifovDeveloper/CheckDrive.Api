using System;

namespace CheckDrive.ApiContracts.DispatcherReview
{
    public class DispatcherReviewForUpdateDto
    {
        public int Id { get; set; }
        public double FuelSpended { get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public int DispatcherId { get; set; }
        public int OperatorId { get; set; }
        public int MechanicId { get; set; }
        public int DriverId { get; set; }
        public int CarId { get; set; }
        public int MechanicAcceptanceId { get; set; }
        public int MechanicHandoverId { get; set; }
    }
}
