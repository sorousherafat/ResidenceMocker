using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class PriceChangeEntityMocker : IEntityMocker<PriceChange>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Residence> _residenceProvider;

    public PriceChangeEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Residence> residenceProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _residenceProvider = residenceProvider;
    }

    public PriceChange MockEntity(int id)
    {
        var residence = _residenceProvider.Provide();
        var hasEndTime = _randomDataGenerator.NextBool(0.3);

        return new PriceChange
        {
            CreatedAt = _randomDataGenerator.NextEventDateTime(),
            StartTime = _randomDataGenerator.NextEventDateTime(),
            EndTime = hasEndTime ? _randomDataGenerator.NextEventDateTime() : null,
            Factor = _randomDataGenerator.NextDecimal(-0.9m, 0.9m),
            ResidenceId = residence.Id
        };
    }
}