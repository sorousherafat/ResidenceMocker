using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;

namespace ResidenceMocker.Providers.Deterministic;

public class AccountEntityProvider : IEntityProvider<Account>
{
    private readonly ResidenceContext _dbContext;
    private IList<Account>? _accounts;
    private int _size;
    private int _index;

    public AccountEntityProvider(ResidenceContext dbContext)
    {
        _dbContext = dbContext;
        _index = 0;
    }

    public Account Provide()
    {
        if (_accounts is null)
        {
            _accounts =_dbContext.Accounts.AsNoTracking().ToList();
            _size = _accounts.Count;
        }

        var account = _accounts[_index];
        _index = (_index + 1) % _size;
        return account;
    }
}