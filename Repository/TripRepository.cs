using go_bus_backend.Data;
using go_bus_backend.Dto;
using go_bus_backend.Dto.Route;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class TripRepository : ITripRepository
{
    private readonly DataContext _context;
    private readonly IBusStopRepository _busStopRepository;

    public TripRepository(DataContext context, IBusStopRepository busStopRepository)
    {
        _context = context;
        _busStopRepository = busStopRepository;
    }

    public async Task<TripGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10)
    {
        var trips = _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment)
            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            trips = trips.Where(x =>
                x.DepartureDate.ToString().Contains(filterQuery) ||
                x.Id.ToString() == filterQuery ||
                x.Route.RouteName.Contains(filterQuery)
            );
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                trips = isAscending ? trips.OrderBy(x => x.Id) : trips.OrderByDescending(x => x.Id);
            }
        }

        // Calculate the total count of buses (before pagination)
        int totalCount = await trips.CountAsync();

        // Pagination
        var skipResults = (page - 1) * pageSize;
        List<Trip> pagedTrips = await trips.Skip(skipResults).Take(pageSize).ToListAsync();

        // Calculate the biggest page number
        var biggestPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
        var getAllAsyncDto = new TripGetAllAsyncDto()
        {
            Trips = pagedTrips,
            BiggestPageNumber = biggestPageNumber
        };

        return getAllAsyncDto;
    }

    public async Task<Trip?> GetById(int id)
    {
        var trip = await _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment)
            .FirstOrDefaultAsync(x => x.Id == id);

        return trip;
    }

    //
    public async Task<Trip> CreateAsync(Trip trip)
    {
        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();
        return trip;
    }

    public async Task<Trip?> UpdateAsync(int id, Trip trip)
    {
        var existingTrip = await _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment).FirstOrDefaultAsync(x => x.Id == id);
        if (existingTrip == null)
        {
            return null;
        }

        existingTrip.TripSegments = trip.TripSegments;
        existingTrip.Route = trip.Route;
        existingTrip.DepartureDate = trip.DepartureDate;
        existingTrip.PricePerKm = trip.PricePerKm;
        existingTrip.Bus = trip.Bus;


        await _context.SaveChangesAsync();
        return existingTrip;
    }

    public async Task<Trip?> DeleteAsync(int id)
    {
        var existingTrip = await _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (existingTrip == null)
        {
            return null;
        }


        await _context.SaveChangesAsync();


        foreach (var tripSegment in existingTrip.TripSegments)
        {
            _context.TripSegments.Remove(tripSegment);
        }

        _context.Trips.Remove(existingTrip);
        await _context.SaveChangesAsync();

        return existingTrip;
    }

    public async Task<TripSegment?> CreateTripSegmentAsync(TripSegment tripSegment)
    {
        await _context.TripSegments.AddAsync(tripSegment);
        await _context.SaveChangesAsync();
        return tripSegment;
    }

    public async Task<TripSegment?> GetTripSegmentById(int id)
    {
        var tripSegment = await _context.TripSegments
            .Include(t => t.RouteSegment)
            .ThenInclude(r => r.ArrivalStop)
            .Include(t => t.RouteSegment)
            .ThenInclude(r => r.DepartureStop)
            .FirstOrDefaultAsync(x => x.Id == id);
        return tripSegment;
    }

    public async Task<List<Trip>?> FindTripsByBusStops(BusStop departureStop, BusStop arrivalStop)
    {
        var trips = await _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment)
            .ToListAsync();

        var matchingTrips = trips.Where(trip =>
        {
            var routeSegments = trip.Route.RouteSegments.OrderBy(segment => segment.Id).ToList();
            var startIndex = routeSegments.FindIndex(segment => segment.DepartureStopId == departureStop.Id);
            var endIndex = routeSegments.FindIndex(segment => segment.ArrivalStopId == arrivalStop.Id);

            return startIndex != -1 && endIndex != -1 && endIndex > startIndex;
        }).ToList();


        return matchingTrips;
    }

    public async Task<List<TripSegment>> GetListofTripSegmentsByStops(int departureStopId, int arrivalStopId,
        int tripId)
    {
        var trip = await _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment).ThenInclude(r => r.DepartureStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment).ThenInclude(r => r.ArrivalStop)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null)
        {
            return null;
        }

        bool foundDeparture = false;
        var tripSegments = new List<TripSegment>();
        foreach (var tripSegment in trip.TripSegments)
        {
            if (tripSegment.RouteSegment.DepartureStopId == departureStopId)
            {
                foundDeparture = true;
            }

            if (foundDeparture)
            {
                tripSegments.Add(tripSegment);
            }

            if (tripSegment.RouteSegment.ArrivalStopId == arrivalStopId)
            {
                foundDeparture = false;
            }
        }


        return tripSegments;
    }

    public async Task<TimeSpan> CalculateDurationOfTrip(int departureStopId, int arrivalStopId, int tripId)
    {
        var tripSegments = await GetListofTripSegmentsByStops(departureStopId, arrivalStopId, tripId);
        TimeSpan duration = new TimeSpan(0);
        foreach (var tripSegment in tripSegments)
        {
            duration = duration + tripSegment.RouteSegment.Duration;
        }

        return duration;
    }

    public async Task<DateTime> getStartDateTimeOfTheTrip(int busStopId, int tripId)
    {
        var trip = await GetById(tripId);

        bool foundDeparture = true;
        var departureDateTime = trip.DepartureDate;
        foreach (var tripSegment in trip.TripSegments)
        {
            if (foundDeparture)
            {
                departureDateTime = departureDateTime + tripSegment.RouteSegment.Duration;
            }

            if (tripSegment.RouteSegment.DepartureStopId == busStopId)
            {
                foundDeparture = false;
            }
        }

        return departureDateTime;
    }

    public async Task<DateTime> getEndDateTimeOfTheTrip(int arrivalbusStopId, int departurebusStopId, int tripId)
    {
        var startDate = await getStartDateTimeOfTheTrip(departurebusStopId, tripId);
        var duration = await CalculateDurationOfTrip(departurebusStopId, arrivalbusStopId, tripId);

        return startDate + duration;
    }

    public async Task<bool> CheckEnoughSpaceInBus(int departureStopId, int arrivalStopId, int tripId,
        int passangerCount)
    {
        var trip = await GetById(tripId);

        var tripSegments = await GetListofTripSegmentsByStops(departureStopId, arrivalStopId, tripId);
        bool isAvailableFlag = true;
        var foundDeparture = false;

        foreach (var tripSegment in tripSegments)
        {
            if (tripSegment.RouteSegment.DepartureStopId == departureStopId)
            {
                foundDeparture = true;
            }

            if (foundDeparture)
            {
                if (tripSegment.PassangerCount + passangerCount > trip.Bus.Capacity)
                {
                    isAvailableFlag = false;
                }
            }

            if (tripSegment.RouteSegment.ArrivalStopId == arrivalStopId)
            {
                foundDeparture = false;
            }
        }

        return isAvailableFlag;
    }


    public async Task<Trip> AddPassangerToTripAsync(int tripId, int passengerId,
        int departureBusStop, int arrivalBusStop)
    {
        var trip = await _context.Trips
            .Include(r => r.Bus)
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments)
            .ThenInclude(rs => rs.DepartureStop) // Include DepartureStop from RouteSegment
            .Include(t => t.Route)
            .ThenInclude(r => r.RouteSegments).ThenInclude(r => r.ArrivalStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment).ThenInclude(r => r.DepartureStop)
            .Include(r => r.TripSegments)
            .ThenInclude(r => r.RouteSegment).ThenInclude(r => r.ArrivalStop)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null)
        {
            return null;
        }

        bool foundDeparture = false;

        foreach (var tripSegment in trip.TripSegments)
        {
            if (tripSegment.RouteSegment.DepartureStopId == departureBusStop)
            {
                foundDeparture = true;
            }

            if (foundDeparture)
            {
                ++tripSegment.PassangerCount;
            }

            if (tripSegment.RouteSegment.ArrivalStopId == arrivalBusStop)
            {
                foundDeparture = false;
            }
        }

        await _context.SaveChangesAsync();

        return trip;
    }

    public async Task<ICollection<Trip>?> GetAllTrips(int departureStopId, int arrivalStopId,
        DateOnly departureDate,
        int passangerCount)
    {
        var derpartureStop = await _busStopRepository.GetById(departureStopId);
        var arrivalStop = await _busStopRepository.GetById(arrivalStopId);

        // Filter by departure and arrival bus
        var trips = await FindTripsByBusStops(derpartureStop, arrivalStop);

        if (trips == null)
            return null;

        // Filter by departure date
        trips = trips.Where(trip => DateOnly.FromDateTime(trip.DepartureDate) == departureDate).ToList();
        // Filter by available space in the bus
        var tempTrips = new List<Trip>();
        bool foundDeparture = false;
        foreach (var trip in trips)
        {
            var isAvailable = await CheckEnoughSpaceInBus(departureStopId, arrivalStopId, trip.Id, passangerCount);
            if (isAvailable)
                tempTrips.Add(trip);
        }

        trips = tempTrips;
        return trips;
    }
}