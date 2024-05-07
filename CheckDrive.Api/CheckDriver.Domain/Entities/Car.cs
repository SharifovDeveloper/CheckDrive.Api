namespace CheckDriver.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public double FuelConsumptionFor100Km { get; set; }
        public double FuelTankCapacity { get; set; }
        public int ManufacturedYear { get; set; }

        public List<MechanicHandover> MechanicHandovers { get; set; }
    }
}
