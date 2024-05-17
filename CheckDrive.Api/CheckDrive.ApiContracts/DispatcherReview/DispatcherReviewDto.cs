using System;

namespace CheckDrive.ApiContracts.DispatcherReview
{
    public class DispatcherReviewDto
    {
        public int Id { get; set; }
        public double FuelSpended { get; set; }
        public double DistanceCovered { get; set; }
        public DateTime Date { get; set; }
        public int DispatcherId { get; set; }
        public int OperatorId { get; set; }
        public int MechanicId { get; set; }
        public int DriverId { get; set; }
    }
}
