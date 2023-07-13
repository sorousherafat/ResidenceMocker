using ResidenceMocker.Entities;
using ResidenceMocker.Enums;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class ResidenceEntityMocker : IEntityMocker<Residence>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Host> _hostProvider;
    private readonly IRandomEntityProvider<Address> _addressProvider;

    public ResidenceEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Host> hostProvider, IRandomEntityProvider<Address> addressProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _hostProvider = hostProvider;
        _addressProvider = addressProvider;
    }

    public Residence MockEntity(int id)
    {
        var host = _hostProvider.Provide();
        var address = _addressProvider.Provide();
        var bedsNo = _randomDataGenerator.Next(1, 7);

        return new Residence
        {
            AddressId = address.Id,
            HostNationalId = host.NationalId,
            Area = _randomDataGenerator.NextDecimal(40m, 300m),
            Capacity = _randomDataGenerator.NextShort(1, 6),
            Facility = _randomDataGenerator.NextAlphaString(),
            Price = _randomDataGenerator.NextBool(0.9) ? _randomDataGenerator.Next(100000, 10000000) : null,
            Title = _randomDataGenerator.NextBool(0.8) ? _randomDataGenerator.NextAlphaString() : null,
            HasParking = _randomDataGenerator.NextBool(0.4),
            HasWifi = _randomDataGenerator.NextBool(0.4),
            CancellationPolicy = "[]",
            CreatedAt = _randomDataGenerator.NextEventDateTime(),
            RentFee = _randomDataGenerator.NextBool(0.2) ? _randomDataGenerator.Next(1000000) : null,
            RoomsNo = _randomDataGenerator.NextShort(1, 5),
            CheckInTime = _randomDataGenerator.NextTimeOnly(),
            CheckOutTime = _randomDataGenerator.NextTimeOnly(),
            BuildingType = _randomDataGenerator.Pick<BuildingType>(),
            BedsType = _randomDataGenerator.Pick<BedType>(bedsNo)
        };
    }
}