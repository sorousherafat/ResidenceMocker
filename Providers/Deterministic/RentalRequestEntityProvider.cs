using Microsoft.EntityFrameworkCore;
using ResidenceMocker.Entities;

namespace ResidenceMocker.Providers.Deterministic;

public class RentalRequestEntityProvider : IEntityProvider<RentalRequest>
{
    private readonly ResidenceContext _dbContext;
    private IList<RentalRequest>? _rentalRequests;
    private int _size;
    private int _index;

    public RentalRequestEntityProvider(ResidenceContext dbContext)
    {
        _dbContext = dbContext;
        _index = 0;
    }

    public RentalRequest Provide()
    {
        if (_rentalRequests is null)
        {
            _rentalRequests = _dbContext.RentalRequests.AsNoTracking().ToList();
            _size = _rentalRequests.Count;
        }

        var account = _rentalRequests[_index];
        _index = (_index + 1) % _size;
        return account;
    }
}