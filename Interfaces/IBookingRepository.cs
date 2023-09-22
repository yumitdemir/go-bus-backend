using go_bus_backend.Models;

namespace go_bus_backend.Interfaces;

public interface IBookingRepository
{
    public Task<Booking> CreateAsync(Booking booking);
    public Task<Booking> UpdateAsync(Booking booking, int id);
    public Task<Booking?> GetById(int id);

    public  Task<Booking?> DeleteAsync(int id);


}