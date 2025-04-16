using System;

namespace TourAgency
{
    public class Tour
    {
        public string TourName { get; set; }
        public string Country { get; set; }
        public DateTime DepartureDate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal Cost { get; set; }
        public bool HasNightTransfers { get; set; }
    }
}