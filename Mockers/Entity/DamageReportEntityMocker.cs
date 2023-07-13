using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class DamageReportEntityMocker : IEntityMocker<DamageReport>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Rent> _rentProvider;

    public DamageReportEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Rent> rentProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _rentProvider = rentProvider;
    }

    public DamageReport MockEntity(int id)
    {
        var rent = _rentProvider.Provide();
        var isAccepted = _randomDataGenerator.NextBool(0.3);
        var estimatedCost = _randomDataGenerator.Next(10000, 10000000);

        return new DamageReport
        {
            RentId = rent.Id,
            EstimatedCost = estimatedCost,
            IsAccepted = isAccepted,
            Title = _randomDataGenerator.NextAlphaString(),
            Description = _randomDataGenerator.NextAlphaString(40),
            FinalCost = isAccepted ? (int?) (_randomDataGenerator.NextDecimal(0m, 1m) * estimatedCost) : null,
            CreatedAt = _randomDataGenerator.NextEventDateTime(),
            ResolvedAt = isAccepted ? _randomDataGenerator.NextEventDateTime() : null
        };
    }
}