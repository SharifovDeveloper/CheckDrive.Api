namespace CheckDrive.Domain.ResourceParameters
{
    public class DispatcherReviewResourceParameters : ResourceParametersBase
    {
        public int? DispatcherId { get; set; }
        public int? OperatorId { get; set; }
        public int? MechanicId { get; set; }
        public int? DriverId { get; set; }
        public double? FuelSpended { get; set; }
        public double? FuelSpendedLessThan { get; set; }
        public double? FuelSpendedGreaterThan { get; set; }
        public double? DistanceCovered { get; set; }
        public double? DistanceCoveredLessThan { get; set; }
        public double? DistanceCoveredThan { get; set; }
        public DateTime? Date { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
