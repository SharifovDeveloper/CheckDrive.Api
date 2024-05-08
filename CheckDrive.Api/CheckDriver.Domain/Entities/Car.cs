using CheckDriver.Domain.Common;

namespace CheckDriver.Domain.Entities
{
    public class Car : EntityBase
    {
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public double MeduimFuelConsumption { get; set; }
        public double FuelTankCapacity { get; set; }
        public int ManufacturedYear { get; set; }

        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
    }
}
