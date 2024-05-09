using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.ResourceParameters
{
    public class OperatorReviewResourceParameters : ResourceParametersBase
    {
        public double? OilAmount { get; set; }
        public double? OilAmountLessThan { get; set; }
        public double? OilAmountGreaterThan { get; set; }
        public Status? Status { get; set; }
        public DateTime? Date { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
