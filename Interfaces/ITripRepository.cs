using go_bus_backend.Dto;
using go_bus_backend.Models;
using go_bus_backend.Models.Trip;

namespace go_bus_backend.Interfaces;

public interface ITripRepository
{
    public Task<TripGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10);

    public Task<Trip?> DeleteAsync(int id);
    public Task<Trip> CreateAsync(Trip trip);
    public Task<Trip?> GetById(int id);
    public Task<TripSegment?> CreateTripSegmentAsync(TripSegment tripSegment);

    public  Task<TripSegment?> GetTripSegmentById(int id);
    public  Task<Trip?> UpdateAsync(int id, Trip trip);
    public  Task<List<Trip>?> FindTripsByBusStops(BusStop departureStop, BusStop arrivalStop);

    public Task<Trip> AddPassangerToTripAsync(int tripId, int PassangerId,
        int departureBusStop, int arrivalBusStop);

}