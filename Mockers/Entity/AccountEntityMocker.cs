using ResidenceMocker.Entities;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class AccountEntityMocker : IEntityMocker<Account>
{
    private readonly IRandomDataGenerator _randomDataGenerator;

    public AccountEntityMocker(IRandomDataGenerator randomDataGenerator)
    {
        _randomDataGenerator = randomDataGenerator;
    }

    public Account MockEntity()
    {
        return new Account
        {
            NationalId = _randomDataGenerator.NextNationalId(),
            FirstName = _randomDataGenerator.NextAlphaString(),
            LastName = _randomDataGenerator.NextAlphaString(),
            PhoneNumber = _randomDataGenerator.NextPhoneNumber(),
            JoinedAt = _randomDataGenerator.NextEventDateTime()
        };
    }
}