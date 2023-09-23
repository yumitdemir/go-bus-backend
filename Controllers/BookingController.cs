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
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
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
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingBooking = await _bookingRepository.GetById(updateBookingRequestDto.BookingId);
        if (existingBooking == null)
        {
            return NotFound();
        }

        existingBooking.Status = BookingStatus.Confirmed;
        var passengers = new List<Passenger>();
        foreach (var passenger in updateBookingRequestDto.Passengers)
        {
            var createdPassenger = await _passengerRepository.CreateAsync(passenger);
            passengers.Add(createdPassenger);
        }

        existingBooking.Passengers = passengers;
        var updatedBooking = await _bookingRepository.UpdateAsync(existingBooking, updateBookingRequestDto.BookingId);

        return Ok(updatedBooking);
    }


    [HttpPost]
    [Route("GetBookingInformation")]
    public async Task<IActionResult> GetBookingInformation([FromBody] GetBookingInformationRequestDto requestDto)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var booking = await _bookingRepository.GetBookingByPnrAndEmail(requestDto.pnr, requestDto.email);
        var tripStart = await _tripRepository.getStartDateTimeOfTheTrip(booking.DepartureBusStopId, booking.TripId);
        var tripEnd =
            await _tripRepository.getEndDateTimeOfTheTrip(booking.ArrivalBusStopId, booking.DepartureBusStopId,
                booking.TripId);
        var email = booking.Passengers[0].Email;
        var phone = booking.Passengers[0].PhoneNumber;
        var bookingPassengerName = booking.Passengers[0].Name + " " + booking.Passengers[0].Surname;

        var answerDto = new GetBookingInformationAnswerDto()
        {
            Booking = booking,
            Email = email,
            Phone = phone,
            TripEnd = tripEnd,
            TripStart = tripStart,
            BookingPassengerName = bookingPassengerName
        };

        return Ok(answerDto);
    }

    [HttpPut]
    [Route("CancelBooking")]
    public async Task<IActionResult> CancelBooking([FromQuery] int bookingId)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var booking = await _bookingRepository.GetById(bookingId);
        if (booking == null)
            return NotFound();

        booking.Status = BookingStatus.Cancelled;
        await _tripRepository.AddPassangerToTripAsync(booking.TripId, booking.DepartureBusStopId, booking.ArrivalBusStopId,
            -booking.PassengerCount);
        var updatedBooking = await _bookingRepository.UpdateAsync(booking, bookingId);


        return Ok(updatedBooking);
    }
}