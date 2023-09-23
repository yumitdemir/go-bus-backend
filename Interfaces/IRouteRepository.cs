using go_bus_backend.Dto.Route;
using Route = go_bus_backend.Models.Route;

namespace go_bus_backend.Interfaces;

public interface IRouteRepository
{
    public Task<Models.Route?> DeleteAsync(int id);

    public Task<Models.Route?> UpdateAsync(int id, Models.Route route);
    public Task<List<Route>> GetAllRoutes();


    public Task<Models.Route> CreateAsync(Models.Route route);
    public Task<Models.Route?> GetById(int id);

    public Task<RouteGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10);
}