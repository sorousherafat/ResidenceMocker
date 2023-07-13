using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class ReviewEntityMocker : IEntityMocker<Review>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Rent> _rentProvider;

    public ReviewEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Rent> rentProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _rentProvider = rentProvider;
    }

    public Review MockEntity(int id)
    {
        var rent = _rentProvider.Provide();
        var hasComment = _randomDataGenerator.NextBool(0.7);

        return new Review
        {
            RentId = rent.Id,
            Rating = _randomDataGenerator.NextShort(1, 6),
            Comment = hasComment ? _randomDataGenerator.NextAlphaString() : null,
            IsByHost = rent.Reviews.Count == 0,
            CreatedAt = _randomDataGenerator.NextEventDateTime()
        };
    }
}