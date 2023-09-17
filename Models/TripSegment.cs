using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace go_bus_backend.Models
{
    public class TripSegment
    {
        public TripSegment()
        {
            PassangerCount = 0;
        }

        [Key]
        public int Id { get; set; }

        // Foreign key for RouteSegment
        public int RouteSegmentId { get; set; }

        // Navigation properties
        public RouteSegment RouteSegment { get; set; }

        public int PassangerCount { get; set; }
    }
}