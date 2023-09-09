﻿using go_bus_backend.Dto;
using go_bus_backend.Models;

namespace go_bus_backend.Interfaces;

public interface IBusRepository
{
    public Task<GetAllAsyncDto?> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
        bool isAscending = true, int page = 1, int pageSize = 10);
    public Task<Bus?> GetById(int id);
    public Task<Bus> CreateAsync(Bus bus);
    public Task<Bus?> UpdateAsync(int id, Bus bus);
    public Task<Bus?> DeleteAsync(int id);
}