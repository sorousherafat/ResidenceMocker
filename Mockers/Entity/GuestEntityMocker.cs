using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Deterministic;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class GuestEntityMocker : IEntityMocker<Guest>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IEntityProvider<Account> _accountProvider;

    public GuestEntityMocker(IRandomDataGenerator randomDataGenerator, IEntityProvider<Account> accountProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _accountProvider = accountProvider;
    }

    public Guest MockEntity(int id)
    {
        var account = _accountProvider.Provide();
        
        return new Guest
        {
            NationalId = account.NationalId,
            Wallet = _randomDataGenerator.Next(1000000)
        };
    }
}