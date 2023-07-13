using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class ProvinceEntityMocker : IEntityMocker<Province>
{
    private readonly IRandomDataGenerator _randomDataGenerator;

    public ProvinceEntityMocker(IRandomDataGenerator randomDataGenerator)
    {
        _randomDataGenerator = randomDataGenerator;
    }

    public Province MockEntity(int id)
    {
        return new Province
        {
            Name = _randomDataGenerator.NextAlphaString()
        };
    }
}