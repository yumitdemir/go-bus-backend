using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace go_bus_backend.Models.Trip
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int BusId { get; set; }
        
        // Navigation properties
        public Route Route { get; set; }
        public ICollection<TripSegment> TripSegments { get; set; }
        public decimal PricePerKm { get; set; }
        public Bus Bus { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}