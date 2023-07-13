using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class UnavailabilityEntityMocker : IEntityMocker<Unavailability>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Residence> _residenceProvider;

    public UnavailabilityEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Residence> residenceProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _residenceProvider = residenceProvider;
    }

    public Unavailability MockEntity(int id)
    {
        var residence = _residenceProvider.Provide();
        var hasEndTime = _randomDataGenerator.NextBool(0.3);

        return new Unavailability
        {
            Id = residence.PriceChanges.Count,
            CreatedAt = _randomDataGenerator.NextEventDateTime(),
            StartTime = _randomDataGenerator.NextEventDateTime(),
            EndTime = hasEndTime ? _randomDataGenerator.NextEventDateTime() : null,
            ResidenceId = residence.Id
        };
    }
}