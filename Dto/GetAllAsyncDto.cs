﻿using go_bus_backend.Models;

namespace go_bus_backend.Dto;

public class GetAllAsyncDto
{
    public ICollection<Bus>? Buses { get; set; }
    public int BiggestPageNumber { get; set; }
}