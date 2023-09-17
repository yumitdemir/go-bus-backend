using System.Text.Json;
using go_bus_backend.Models;

namespace go_bus_backend.Interfaces;

public interface IPassangerRepository
{
    public Task<Passenger?> GetById(int id);
    public  Task<Passenger> CreateAsync(Passenger passenger);
    public Task<Passenger?> UpdateAsync(int id, Passenger passanger);
    public Task<Passenger?> DeleteAsync(int id);

}