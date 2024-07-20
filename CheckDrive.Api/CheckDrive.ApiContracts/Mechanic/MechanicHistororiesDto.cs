using System;

namespace CheckDrive.ApiContracts.Mechanic
{
    public class MechanicHistororiesDto
    {
        public bool? IsChecked { get; set; }
        public string Position { get; set; }
        public string CarName { get; set; }
        public DateTime? Date { get; set; }
        public double Distance { get; set; }
        public string DriverName { get; set; }
        public string Comments { get; set; }
    }
}