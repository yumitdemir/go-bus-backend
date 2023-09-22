﻿using go_bus_backend.Data;
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
        var existingBooking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
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
  

    
    
}