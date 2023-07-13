using ResidenceMocker.Entities;
using ResidenceMocker.Enums;
using ResidenceMocker.Providers.Deterministic;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class RentEntityMocker : IEntityMocker<Rent>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IEntityProvider<RentalRequest> _rentalRequestProvider;

    public RentEntityMocker(IRandomDataGenerator randomDataGenerator, IEntityProvider<RentalRequest> rentalRequestProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _rentalRequestProvider = rentalRequestProvider;
    }

    public Rent MockEntity(int id)
    {
        var rentalRequest = _rentalRequestProvider.Provide();
        var isCancelled = _randomDataGenerator.NextBool(0.1);
        var cancellationPenaltyCandidate = (int) _randomDataGenerator.NextDecimal(0m, 1m) * rentalRequest.RawPrice;
        var cancellationPenalty = isCancelled ? cancellationPenaltyCandidate : 0;

        return new Rent
        {
            Id = rentalRequest.Id,
            Status = _randomDataGenerator.Pick<RentStatus>(),
            CancellationPolicy = "[]",
            CancellationPenalty = cancellationPenalty,
            CancellationTimestamp = isCancelled ? _randomDataGenerator.NextEventDateTime() : null,
            FinalPrice = rentalRequest.RawPrice - cancellationPenalty
        };
    }
}