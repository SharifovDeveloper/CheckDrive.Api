using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class Car : EntityBase
    {
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Mileage { get; set; }
        public double MeduimFuelConsumption { get; set; }
        public double FuelTankCapacity { get; set; }
        public double RemainingFuel { get; set; }
        public int ManufacturedYear { get; set; }

        public virtual ICollection<DispatcherReview> Reviewers { get; set; }
        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
        public virtual ICollection<MechanicAcceptance> MechanicAcceptance { get; set; }
        public virtual ICollection<OperatorReview> OperatorReviews { get; set; }
    }
}
