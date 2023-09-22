using go_bus_backend.Data;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Services;

public class DeleteExpiredBookingsService : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public DeleteExpiredBookingsService(ILogger<DeleteExpiredBookingsService> logger, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    private async void DoWork(object? state)
    {
        _logger.LogInformation("Timed Background Service is working.");

        using var scope = _scopeFactory.CreateScope();
        var tripRepository = scope.ServiceProvider.GetRequiredService<ITripRepository>();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
        var currentTime = DateTime.Now;
        // Get bookings from your database that are older than 10 minutes
        var oldBookings = context.Bookings
            .Where(b => b.InitializationTime.AddMinutes(10) < currentTime && b.Status == BookingStatus.Pending).ToList();
        foreach (var booking in oldBookings)
        {
            await tripRepository.AddPassangerToTripAsync(booking.TripId,
                booking.DepartureBusStopId, booking.ArrivalBusStopId,
                -booking.PassengerCount);
            await bookingRepository.DeleteAsync(booking.Id);
            
        }

        Console.WriteLine("test");
        Console.WriteLine(oldBookings.Count);
    }



    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is starting.");

        // Set up a timer to run your task every minute (adjust the interval as needed)
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is stopping.");

        // Stop the timer when the service is stopped
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}