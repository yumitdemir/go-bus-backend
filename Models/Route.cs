using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace go_bus_backend.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        
        // Navigation properties
        public string RouteName { get; set; }
        public ICollection<RouteSegment> RouteSegments { get; set; }
    }
}