namespace CheckDrive.Domain.DTOs.Car;
public record CarForCreateDto(
    string Model,
    string Color,
    string Number,
    double MeduimFuelConsumption,
    double FuelTankCapacity,
    int ManufacturedYear);