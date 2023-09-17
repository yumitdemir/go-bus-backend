using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace go_bus_backend.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; }
        public bool DriverStatus { get; set; }

        
    }
}