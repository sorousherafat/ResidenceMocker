using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ResidenceMocker.Mockers.Entity;

namespace ResidenceMocker.Mockers;

public class Mocker : IMocker
{
    private readonly IHostApplicationLifetime _lifeTime;
    private readonly IServiceProvider _serviceProvider;
    private readonly ResidenceContext _dbContext;
    private readonly ILogger<Mocker> _logger;
    private readonly IConfiguration _configuration;

    public Mocker(IHostApplicationLifetime lifeTime, IServiceProvider serviceProvider, ResidenceContext dbContext,
        ILogger<Mocker> logger, IConfiguration configuration)
    {
        _lifeTime = lifeTime;
        _serviceProvider = serviceProvider;
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task MockAsync()
    {
        _logger.LogInformation("Started mocking");
        
        await MockEntitiesAsync(_dbContext.Accounts, Convert.ToInt32(_configuration["Mock:GuestCount"]) + Convert.ToInt32(_configuration["Mock:HostCount"]));
        await MockEntitiesAsync(_dbContext.Hosts, Convert.ToInt32(_configuration["Mock:HostCount"]));
        await MockEntitiesAsync(_dbContext.Guests, Convert.ToInt32(_configuration["Mock:GuestCount"]));
        await MockEntitiesAsync(_dbContext.Provinces, Convert.ToInt32(_configuration["Mock:ProvinceCount"]));
        await MockEntitiesAsync(_dbContext.Cities, Convert.ToInt32(_configuration["Mock:CityCount"]));
        await MockEntitiesAsync(_dbContext.Addresses, Convert.ToInt32(_configuration["Mock:AddressCount"]));
        await MockEntitiesAsync(_dbContext.Residences, Convert.ToInt32(_configuration["Mock:ResidenceCount"]));
        await MockEntitiesAsync(_dbContext.RentalRequests, Convert.ToInt32(_configuration["Mock:RentalRequestCount"]));
        await MockEntitiesAsync(_dbContext.Rents, Convert.ToInt32(_configuration["Mock:RentCount"]));
        await MockEntitiesAsync(_dbContext.Complaints, Convert.ToInt32(_configuration["Mock:ComplaintCount"]));
        await MockEntitiesAsync(_dbContext.DamageReports, Convert.ToInt32(_configuration["Mock:DamageReportCount"]));
        await MockEntitiesAsync(_dbContext.Messages, Convert.ToInt32(_configuration["Mock:MessageCount"]));
        await MockEntitiesAsync(_dbContext.PriceChanges, Convert.ToInt32(_configuration["Mock:PriceChangeCount"]));
        await MockEntitiesAsync(_dbContext.Reviews, Convert.ToInt32(_configuration["Mock:ReviewCount"]));
        await MockEntitiesAsync(_dbContext.Unavailabilities, Convert.ToInt32(_configuration["Mock:UnavailabilityCount"]));

        Environment.Exit(0);
    }

    private async Task MockEntitiesAsync<T>(DbSet<T> dbSet, int count) where T : class
    {
        var entityMocker = _serviceProvider.GetService<IEntityMocker<T>>();
        Debug.Assert(entityMocker != null, nameof(entityMocker) + " != null");

        dbSet.AddRange(Enumerable.Range(1, count).Select(i => entityMocker.MockEntity(i)));
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Mocked {FullName} entities", typeof(T).FullName);
    }
}