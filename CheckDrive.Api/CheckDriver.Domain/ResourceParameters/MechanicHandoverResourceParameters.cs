﻿using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.ResourceParameters
{
    public class MechanicHandoverResourceParameters : ResourceParametersBase
    {
        public bool? IsHanded { get; set; }
        public Status? Status { get; set; }
        public DateTime? Date { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
