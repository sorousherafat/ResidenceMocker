using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Providers.Random;

public class RandomAddressProvider : IRandomEntityProvider<Address>
{
    private readonly ResidenceContext _dbContext;
    private readonly IRandomDataGenerator _randomDataGenerator;
    private IList<Address>? _addresses;

    public RandomAddressProvider(ResidenceContext dbContext, IRandomDataGenerator randomDataGenerator)
    {
        _dbContext = dbContext;
        _randomDataGenerator = randomDataGenerator;
    }

    public Address Provide()
    {
        _addresses ??= _dbContext.Addresses.ToList();

        return _randomDataGenerator.Pick(_addresses);
    }
}