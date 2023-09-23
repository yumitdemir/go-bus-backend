using go_bus_backend.Data;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly DataContext _context;

    public BookingRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking> UpdateAsync(Booking booking, int id)
    {
        var existingBooking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBooking != null)
        {
            existingBooking.ArrivalBusStop = booking.ArrivalBusStop;
            existingBooking.DepartureBusStop = booking.DepartureBusStop;
            existingBooking.Trip = booking.Trip;
            existingBooking.Status = booking.Status;
            existingBooking.Passengers = booking.Passengers;
            existingBooking.PassengerCount = booking.PassengerCount;
        }

        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking?> GetById(int id)
    {
        var existingBooking = await _context.Bookings.Include(x => x.Passengers).Include(x => x.ArrivalBusStop)
            .Include(x => x.DepartureBusStop).Include(x => x.Trip).Include(x => x.Trip).ThenInclude(x => x.TripSegments)
            .Include(x => x.Trip).ThenInclude(x => x.Bus).Include(x => x.Trip).ThenInclude(x => x.Route).ThenInclude(x=>x.RouteSegments)
            .FirstOrDefaultAsync(x => x.Id == id);
        return existingBooking;
    }

    public async Task<Booking?> DeleteAsync(int id)
    {
        var existingBooking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBooking == null)
        {
            return null;
        }

        _context.Remove(existingBooking);
        await _context.SaveChangesAsync();
        return existingBooking;
    }

    public async Task<List<Booking>?> DeleteRangeAsync(List<Booking> bookings)
    {
        _context.Bookings.RemoveRange(bookings);
        await _context.SaveChangesAsync();
        return bookings;
    }

    public async Task<Booking?> GetBookingByPnrAndEmail(string pnr, string email)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(x => x.PNR == pnr);

        var booking = await _context.Bookings.FirstOrDefaultAsync(x =>
            trip != null && x.TripId == trip.Id && x.Passengers[0].Email == email);
        booking = await GetById(booking.Id);
        return booking;
    }
}