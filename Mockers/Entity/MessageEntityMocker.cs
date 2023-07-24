using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class MessageEntityMocker : IEntityMocker<Message>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Host> _hostProvider;
    private readonly IRandomEntityProvider<Guest> _guestProvider;

    public MessageEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Host> hostProvider, IRandomEntityProvider<Guest> guestProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _hostProvider = hostProvider;
        _guestProvider = guestProvider;
    }

    public Message MockEntity()
    {
        var host = _hostProvider.Provide();
        var guest = _guestProvider.Provide();

        return new Message
        {
            HostNationalId = host.NationalId,
            GuestNationalId = guest.NationalId,
            SentByHost = _randomDataGenerator.NextBool(),
            SentAt = _randomDataGenerator.NextEventDateTime(),
            Text = _randomDataGenerator.NextAlphaString(20)
        };
    }
}