using System.Text.Json;
using go_bus_backend.Data;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class PassangerRepository : IPassangerRepository
{
    private readonly DataContext _context;

    public PassangerRepository(DataContext context)
    {
        _context = context;
    }
   
    public async Task<Passenger?> GetById(int id)
    {
        var passanger = await _context.Passengers.FirstOrDefaultAsync(x => x.Id == id);

        return passanger;
    }

    public async Task<Passenger> CreateAsync(Passenger passenger)
    {
        await _context.Passengers.AddAsync(passenger);
        await _context.SaveChangesAsync();
        return passenger;
    }



    public async Task<Passenger?> UpdateAsync(int id, Passenger passanger)
    {
        var existingPassanger = await _context.Passengers.FirstOrDefaultAsync(x => x.Id == id);
        if (existingPassanger == null)
        {
            return null;
        }

        existingPassanger.Email = passanger.Email;
        existingPassanger.Name = passanger.Name;
        existingPassanger.Surname = passanger.Surname;
        existingPassanger.PhoneNumber = passanger.PhoneNumber;
        existingPassanger.SeatNumber = passanger.SeatNumber;


        await _context.SaveChangesAsync();
        return existingPassanger;
    }

    public async Task<Passenger?> DeleteAsync(int id)
    {
        var existingPassanger = await _context.Passengers.FirstOrDefaultAsync(x => x.Id == id);
        if (existingPassanger == null)
        {
            return null;
        }
        _context.Remove(existingPassanger);
        await _context.SaveChangesAsync();
        return existingPassanger;
    }
    
    
}