using ResidenceMocker.Entities;
using ResidenceMocker.Providers.Random;
using ResidenceMocker.Randoms;

namespace ResidenceMocker.Mockers.Entity;

public class MessageEntityMocker : IEntityMocker<Message>
{
    private readonly IRandomDataGenerator _randomDataGenerator;
    private readonly IRandomEntityProvider<Account> _accountProvider;

    public MessageEntityMocker(IRandomDataGenerator randomDataGenerator, IRandomEntityProvider<Account> accountProvider)
    {
        _randomDataGenerator = randomDataGenerator;
        _accountProvider = accountProvider;
    }

    public Message MockEntity(int id)
    {
        var sender = _accountProvider.Provide();
        var receiver = _accountProvider.Provide();

        return new Message
        {
            SenderNationalId = sender.NationalId,
            ReceiverNationalId = receiver.NationalId,
            SentAt = _randomDataGenerator.NextEventDateTime(),
            Text = _randomDataGenerator.NextAlphaString(20)
        };
    }
}