using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Deterministic;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class HostEntityMocker : IEntityMocker<Host>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IEntityProvider<Account> _accountProvider;

    public HostEntityMocker(IRandomDataGenerator randomDataGenerator, IEntityProvider<Account> accountProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _accountProvider = accountProvider;
    }

    public Host MockEntity(int id)
    {
        var account = _accountProvider.Provide();
        var isVerified = _randomDataGenerator.NextBool();
        
        return new Host
        {
            NationalId = account.NationalId,
            IsVerified = isVerified,
            VerifiedAt = isVerified ? _randomDataGenerator.NextEventDateTime() : null,
        };
    }
}