using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomGuestProvider : IRandomEntityProvider<Guest>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Guest>? _guests;

    public RandomGuestProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Guest Provide()
    {
        _guests ??= _dbContext.Guests.AsNoTracking().ToList();

        return _randomDataGenerator.Pick(_guests);
    }
}