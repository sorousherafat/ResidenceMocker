using ResidenceMocker.Entities;
using ResidenceMocker.Enums;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers;

public class RentalRequestEntityMocker : IEntityMocker<RentalRequest>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Residence> _residenceProvider;
    private readonly IRandomEntityProvider<Guest> _guestProvider;

    public RentalRequestEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Residence> residenceProvider, IRandomEntityProvider<Guest> guestProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _residenceProvider = residenceProvider;
        _guestProvider = guestProvider;
    }

    public RentalRequest MockEntity(int id)
    {
        var residence = _residenceProvider.Provide();
        var guest = _guestProvider.Provide();
        var startDate = _randomDataGenerator.NextEventDateOnly();
        var length = _randomDataGenerator.Next(1, 14);
        var endDate = startDate.AddDays(length);
        var isResolved = _randomDataGenerator.NextBool(0.7);

        return new RentalRequest
        {
            CreatedAt = _randomDataGenerator.NextEventDateTime(),
            StartDate = startDate,
            EndDate = endDate,
            GuestNationalId = guest.NationalId,
            GuestsNo = _randomDataGenerator.NextShort(1, 6),
            ResidenceId = residence.Id,
            RawPrice = residence.Price * length + residence.RentFee ?? 0,
            ResolvedAt = isResolved ? _randomDataGenerator.NextEventDateTime() : null,
            Status = _randomDataGenerator.Pick<RentalRequestStatus>()
        };
    }
}