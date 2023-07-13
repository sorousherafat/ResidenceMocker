using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomRentalRequestProvider : IRandomEntityProvider<RentalRequest>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<RentalRequest>? _rentalRequests;

    public RandomRentalRequestProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public RentalRequest Provide()
    {
        _rentalRequests ??= _dbContext.RentalRequests.AsNoTracking().ToList();

        return _randomDataGenerator.Pick(_rentalRequests);
    }
}