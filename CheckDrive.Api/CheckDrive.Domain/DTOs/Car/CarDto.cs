namespace CheckDrive.Domain.DTOs.Car;
public record CarDto(
    int Id,
    string Model,
    string Color,
    string Number,
    double MeduimFuelConsumption,
    double FuelTankCapacity,
    int ManufacturedYear);
