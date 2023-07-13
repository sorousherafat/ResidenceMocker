using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class ComplaintEntityMocker : IEntityMocker<Complaint>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Rent> _rentProvider;

    public ComplaintEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Rent> rentProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _rentProvider = rentProvider;
    }

    public Complaint MockEntity(int id)
    {
        var rent = _rentProvider.Provide();
        var isResolved = _randomDataGenerator.NextBool(0.3);

        return new Complaint
        {
            RentId = rent.Id,
            IsAccepted = _randomDataGenerator.NextBool(0.3),
            Title = _randomDataGenerator.NextAlphaString(),
            Description = _randomDataGenerator.NextAlphaString(40),
            CreatedAt = _randomDataGenerator.NextEventDateTime(),
            ResolvedAt = isResolved ? _randomDataGenerator.NextEventDateTime() : null
        };
    }   
}