using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using ResidenceMocker.Entities;
using ResidenceMocker.Enums;
using ResidenceMocker.Mockers;
using ResidenceMocker.Mockers.Entity;
using ResidenceMocker.Providers.Deterministic;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;
using HostEntity = ResidenceMocker.Entities.Host;

namespace ResidenceMocker;

public static class ResidenceServiceCollectionExtensions
{
    public static void AddResidenceServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddResidenceDbContext(configuration);
        serviceCollection.AddLogging();
        serviceCollection.AddBaseServices();
        serviceCollection.AddEntityMockers();
        serviceCollection.AddRandomEntityProviders();
        serviceCollection.AddSequentialEntityProviders();
    }

    private static void AddResidenceDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));

        dataSourceBuilder.MapEnum<BedType>();
        dataSourceBuilder.MapEnum<BuildingType>();
        dataSourceBuilder.MapEnum<RentalRequestStatus>();
        dataSourceBuilder.MapEnum<RentStatus>();

        var dataSource = dataSourceBuilder.Build();

        serviceCollection.AddDbContext<ResidenceContext>(options => options.UseNpgsql(dataSource));
    }

    private static void AddSequentialEntityProviders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IEntityProvider<Account>, AccountEntityProvider>();
        serviceCollection.AddTransient<IEntityProvider<RentalRequest>, RentalRequestEntityProvider>();
    }

    private static void AddRandomEntityProviders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRandomEntityProvider<Account>, RandomAccountProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<Address>, RandomAddressProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<City>, RandomCityProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<Guest>, RandomGuestProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<HostEntity>, RandomHostProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<Province>, RandomProvinceProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<RentalRequest>, RandomRentalRequestProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<Rent>, RandomRentProvider>();
        serviceCollection.AddScoped<IRandomEntityProvider<Residence>, RandomResidenceProvider>();
    }

    private static void AddEntityMockers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEntityMocker<Account>, AccountEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Address>, AddressEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<City>, CityEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Complaint>, ComplaintEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<DamageReport>, DamageReportEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Guest>, GuestEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<HostEntity>, HostEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Message>, MessageEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<PriceChange>, PriceChangeEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Province>, ProvinceEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Rent>, RentEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<RentalRequest>, RentalRequestEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Residence>, ResidenceEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Review>, ReviewEntityMocker>();
        serviceCollection.AddScoped<IEntityMocker<Unavailability>, UnavailabilityEntityMocker>();
    }

    private static void AddBaseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRandomDataGenerator, RandomDataGenerator>();
        serviceCollection.AddScoped<IMocker, Mocker>();
    }
}