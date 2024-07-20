﻿namespace CheckDrive.Domain.ResourceParameters
{
    public class DispatcherResourceParameters : ResourceParametersBase
    {
        public int? AccountId { get; set; }
        public override string OrderBy { get; set; } = "id";
    }
}
