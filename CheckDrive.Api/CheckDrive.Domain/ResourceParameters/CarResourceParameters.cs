namespace CheckDrive.Domain.ResourceParameters
{
    public class CarResourceParameters : ResourceParametersBase
    {
        public double? MeduimFuelConsumption { get; set; }
        public double? MeduimFuelConsumptionLessThan { get; set; }
        public double? MeduimFuelConsumptionGreaterThan { get; set; }
        public double? FuelTankCapacity { get; set; }
        public double? FuelTankCapacityLessThan { get; set; }
        public double? FuelTankCapacityThan { get; set; }
        public int? ManufacturedYear { get; set; }
        public int? ManufacturedYearLessThan { get; set; }
        public int? ManufacturedYearGreaterThan { get; set; }
        public override string OrderBy { get; set; } = "model";
    }
}
