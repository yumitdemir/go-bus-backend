using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly ITripRepository _tripRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly IBusRepository _busRepository;
    private readonly IBusStopRepository _busStopRepository;

    public TripController(ITripRepository tripRepository, IRouteRepository routeRepository,
        IBusRepository busRepository, IBusStopRepository busStopRepository)
    {
        _busStopRepository = busStopRepository;
        _busRepository = busRepository;
        _routeRepository = routeRepository;
        _tripRepository = tripRepository;
    }

    [HttpGet]
    [Route("GetTripsByFilters")]
    public async Task<IActionResult> GetTripsByFilters([FromQuery] TripSearchRequestDto tripSearchRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var trips = await _tripRepository.GetAllTrips(tripSearchRequestDto.departureStopId,
            tripSearchRequestDto.arrivalStopId, DateOnly.Parse(tripSearchRequestDto.departureDate),
            tripSearchRequestDto.passangerCount);
        var tripAnswerList = new List<TripSearchAnswerDto>();
        foreach (var trip in trips)
        {
            var tripSearchAnswerDto = new TripSearchAnswerDto()
            {
                TripId = trip.Id,
                ArrivalStop = await _busStopRepository.GetById(tripSearchRequestDto.arrivalStopId),
                DepartureStop = await _busStopRepository.GetById(tripSearchRequestDto.departureStopId),
                Duration = await _tripRepository.CalculateDurationOfTrip(tripSearchRequestDto.departureStopId,
                    tripSearchRequestDto.arrivalStopId, trip.Id),
                DepartureTime =
                    await _tripRepository.getStartDateTimeOfTheTrip(tripSearchRequestDto.departureStopId, trip.Id),
                ArrivalTime = await _tripRepository.getEndDateTimeOfTheTrip(tripSearchRequestDto.arrivalStopId,
                    tripSearchRequestDto.departureStopId, trip.Id),
                Bus = trip.Bus,
                Price = await _tripRepository.CalculatePriceOfTrip(tripSearchRequestDto.departureStopId,
                    tripSearchRequestDto.arrivalStopId, trip.Id)
            };
            tripAnswerList.Add(tripSearchAnswerDto);
        }


        return Ok(tripAnswerList);
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Create([FromBody] AddTripRequestDto addTripRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var route = await _routeRepository.GetById(addTripRequestDto.RouteId);
        var bus = await _busRepository.GetById(addTripRequestDto.BusId);
        if (route == null || bus == null)
        {
            return NotFound("Either the route or the bus was not found.");
        }

        var tripsCreated = new List<Trip>();
        // Create not repeating trip
        if (addTripRequestDto.DepartureDate != null)
        {
            var tripSegments = new List<TripSegment>();
            foreach (var routeSegment in route.RouteSegments)
            {
                var tripSegment = new TripSegment()
                {
                    RouteSegment = routeSegment,
                };

                var createdTripSegment = await _tripRepository.CreateTripSegmentAsync(tripSegment);
                if (createdTripSegment != null)
                {
                    tripSegments.Add(createdTripSegment);
                }
                else
                {
                    return BadRequest("Failed to create a Trip Segment");
                }
            }


            var trip = new Trip()
            {
                Route = route,
                Bus = bus,
                DepartureDate = DateTime.Parse(addTripRequestDto.DepartureDate),
                PricePerKm = addTripRequestDto.PricePerKm,
                TripSegments = tripSegments
            };

            var createdTrip = await _tripRepository.CreateAsync(trip);

            return Ok(createdTrip);
        }

        DateTime startDate;
        DateTime lastAvailableDate;
        List<DateTime> unavailableDates;
        List<DayOfWeek> daysOfWeek;
        List<TimeSpan> timesOfDay;
        try
        {
            startDate = DateTime.Parse(addTripRequestDto.StartDate);
            lastAvailableDate = DateTime.Parse(addTripRequestDto.LastAvailableDate);
            unavailableDates = addTripRequestDto.UnavailableDates != null
                ? addTripRequestDto.UnavailableDates.Select(DateTime.Parse).ToList()
                : new List<DateTime>();
            daysOfWeek = addTripRequestDto.DayOfWeek.Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d)).ToList();
            timesOfDay = addTripRequestDto.TimeOfDay.Select(TimeSpan.Parse).ToList();
        }
        catch (FormatException)
        {
            return BadRequest("Data is not in the correct form");
        }


        // Start from the StartDate and create trips until the LastAvailableDate
        for (var date = startDate; date <= lastAvailableDate; date = date.AddDays(1))
        {
            // Check if the current day is one of the selected days
            if (daysOfWeek.Contains(date.DayOfWeek) && !unavailableDates.Contains(date))
            {
                // For each selected time of day, create a new trip
                foreach (var timeOfDay in timesOfDay)
                {
                    var departureDate = date + timeOfDay;

                    var tripSegments = new List<TripSegment>();
                    if (route != null)
                        foreach (var routeSegment in route.RouteSegments)
                        {
                            var tripSegment = new TripSegment()
                            {
                                RouteSegment = routeSegment,
                            };

                            var createdTripSegment = await _tripRepository.CreateTripSegmentAsync(tripSegment);
                            if (createdTripSegment != null) tripSegments.Add(createdTripSegment);
                        }

                    var trip = new Trip()
                    {
                        Route = route,
                        Bus = bus,
                        DepartureDate = departureDate,
                        PricePerKm = addTripRequestDto.PricePerKm,
                        TripSegments = tripSegments
                    };

                    var newTrip = await _tripRepository.CreateAsync(trip);
                    tripsCreated.Add(newTrip);
                }
            }
        }


        return Ok(tripsCreated);
    }

    [HttpPut]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateTripRequestDto updateDriverRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var oldTrip = await _tripRepository.GetById(id);
        if (oldTrip == null)
            return NotFound();

        var route = await _routeRepository.GetById(updateDriverRequestDto.RouteId);
        var bus = await _busRepository.GetById(updateDriverRequestDto.BusId);
        if (route == null || bus == null)
        {
            return NotFound("Either the route or the bus was not found.");
        }

        var tripSegments = new List<TripSegment>();

        foreach (var routeSegment in route.RouteSegments)
        {
            var tripSegment = new TripSegment()
            {
                RouteSegment = routeSegment,
            };
            var createdTripSegment = await _tripRepository.CreateTripSegmentAsync(tripSegment);
            if (createdTripSegment != null)
            {
                tripSegments.Add(createdTripSegment);
            }
            else
            {
                return BadRequest("Failed to create a Trip Segment");
            }
        }

        var trip = new Trip()
        {
            Route = route,
            Bus = bus,
            DepartureDate = updateDriverRequestDto.DepartureDate,
            PricePerKm = updateDriverRequestDto.PricePerKm,
            TripSegments = tripSegments
        };

        var updatedTrip = await _tripRepository.UpdateAsync(id, trip);
        return Ok(updatedTrip);
    }

    [HttpDelete]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var oldTrip = await _tripRepository.GetById(id);
        if (oldTrip == null)
            return NotFound();

        var deletedTrip = await _tripRepository.DeleteAsync(id);
        return Ok(deletedTrip);
    }

    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetDrivers([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var trips = await _tripRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(trips);
    }

    [HttpGet("GetById")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var trip = await _tripRepository.GetById(id);
        return Ok(trip);
    }
  
}