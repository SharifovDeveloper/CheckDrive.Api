namespace CheckDrive.Domain.DTOs.Car;
public record CarForUpdateDto(
    int Id,
    string Model,
    string Color,
    string Number,
    double MeduimFuelConsumption,
    double FuelTankCapacity,
    int ManufacturedYear);