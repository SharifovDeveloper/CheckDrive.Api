namespace CheckDrive.DTOs.Car
{
    public class CarForCreateDto
    {
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public double MeduimFuelConsumption { get; set; }
        public double FuelTankCapacity { get; set; }
        public int ManufacturedYear { get; set; }
    }
}
