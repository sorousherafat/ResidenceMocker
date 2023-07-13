using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using ResidenceMocker;
using ResidenceMocker.Entities;
using ResidenceMocker.Enums;
using ResidenceMocker.Mockers;
using ResidenceMocker.Providers.Deterministic;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;
using HostEntity = ResidenceMocker.Entities.Host;
using Host = Microsoft.Extensions.Hosting.Host;

var builder = Host.CreateApplicationBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();


var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));

dataSourceBuilder.MapEnum<BedType>();
dataSourceBuilder.MapEnum<BuildingType>();
dataSourceBuilder.MapEnum<RentalRequestStatus>();
dataSourceBuilder.MapEnum<RentStatus>();

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<ResidenceContext>(options => options.UseNpgsql(dataSource));
builder.Services.AddLogging();

builder.Services.AddScoped<IRandomDataGenerator, RandomDataGenerator>();
builder.Services.AddScoped<IMocker, Mocker>();

builder.Services.AddScoped<IEntityMocker<Account>, AccountEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Address>, AddressEntityMocker>();
builder.Services.AddScoped<IEntityMocker<City>, CityEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Complaint>, ComplaintEntityMocker>();
builder.Services.AddScoped<IEntityMocker<DamageReport>, DamageReportEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Guest>, GuestEntityMocker>();
builder.Services.AddScoped<IEntityMocker<HostEntity>, HostEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Message>, MessageEntityMocker>();
builder.Services.AddScoped<IEntityMocker<PriceChange>, PriceChangeEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Province>, ProvinceEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Rent>, RentEntityMocker>();
builder.Services.AddScoped<IEntityMocker<RentalRequest>, RentalRequestEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Residence>, ResidenceEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Review>, ReviewEntityMocker>();
builder.Services.AddScoped<IEntityMocker<Unavailability>, UnavailabilityEntityMocker>();

builder.Services.AddScoped<IRandomEntityProvider<Account>, RandomAccountProvider>();
builder.Services.AddScoped<IRandomEntityProvider<Address>, RandomAddressProvider>();
builder.Services.AddScoped<IRandomEntityProvider<City>, RandomCityProvider>();
builder.Services.AddScoped<IRandomEntityProvider<Guest>, RandomGuestProvider>();
builder.Services.AddScoped<IRandomEntityProvider<HostEntity>, RandomHostProvider>();
builder.Services.AddScoped<IRandomEntityProvider<Province>, RandomProvinceProvider>();
builder.Services.AddScoped<IRandomEntityProvider<RentalRequest>, RandomRentalRequestProvider>();
builder.Services.AddScoped<IRandomEntityProvider<Rent>, RandomRentProvider>();
builder.Services.AddScoped<IRandomEntityProvider<Residence>, RandomResidenceProvider>();

builder.Services.AddTransient<IEntityProvider<Account>, AccountEntityProvider>();
builder.Services.AddTransient<IEntityProvider<RentalRequest>, RentalRequestEntityProvider>();

using var host = builder.Build();

var mocker = host.Services.GetService<IMocker>();
Debug.Assert(mocker != null, nameof(mocker) + " != null");
await mocker.MockAsync();

await host.RunAsync();