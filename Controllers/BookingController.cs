using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IBusStopRepository _busStopRepository;
    private readonly IPassangerRepository _passengerRepository;

    public BookingController(IBookingRepository bookingRepository, ITripRepository tripRepository,
        IBusStopRepository busStopRepository, IPassangerRepository passengerRepository)
    {
        _passengerRepository = passengerRepository;
        _busStopRepository = busStopRepository;
        _tripRepository = tripRepository;
        _bookingRepository = bookingRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InitializeBookingRequestDto initializeBookingRequestDto)
    {
        var booking = new Booking()
        {
            Trip = await _tripRepository.GetById(initializeBookingRequestDto.TripId),
            PassengerCount = initializeBookingRequestDto.PassengerCount,
            Status = BookingStatus.Pending,
            ArrivalBusStop = await _busStopRepository.GetById(initializeBookingRequestDto.ArrivalBusStopId),
            DepartureBusStop = await _busStopRepository.GetById(initializeBookingRequestDto.DepartureBusStopId),
            InitializationTime = DateTime.Now
        };
        var createdBooking = await _bookingRepository.CreateAsync(booking);
        
        await _tripRepository.AddPassangerToTripAsync(initializeBookingRequestDto.TripId,
            initializeBookingRequestDto.DepartureBusStopId, initializeBookingRequestDto.ArrivalBusStopId,
            initializeBookingRequestDto.PassengerCount);
        
        
        return Ok(createdBooking);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBookingRequestDto updateBookingRequestDto)
    {
        var existingBooking = await _bookingRepository.GetById(updateBookingRequestDto.BookingId);
        existingBooking.Status = BookingStatus.Confirmed;
        var passengers = new List<Passenger>();
        foreach (var passenger in updateBookingRequestDto.Passengers)
        {
            var createdPassenger = await _passengerRepository.CreateAsync(passenger);
            passengers.Add(createdPassenger);
        }

        existingBooking.Passengers = passengers;
        var updatedBooking = await _bookingRepository.UpdateAsync(existingBooking,updateBookingRequestDto.BookingId);

        return Ok(updatedBooking);
    }
}