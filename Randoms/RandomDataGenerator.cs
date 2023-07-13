namespace ResidenceMocker.Randoms;

public class RandomDataGenerator : IRandomDataGenerator
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private static readonly (DateTime, DateTime) BirthDateTime = (new DateTime(1960, 1, 1), new DateTime(2003, 1, 1));
    private static readonly (DateTime, DateTime) EventDateTime = (new DateTime(2018, 1, 1), new DateTime(2023, 1, 1));
    private readonly Random _random;

    public RandomDataGenerator()
    {
        _random = new Random();
    }

    public int Next() => _random.Next();

    public int Next(int maxValue) => _random.Next(maxValue);

    public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
    
    public short NextShort(short minValue, short maxValue)
    {
        return (short) _random.Next(minValue, maxValue);
    }

    public decimal NextDecimal(decimal minValue, decimal maxValue)
    {
        return new decimal(_random.NextDouble() * (double) (maxValue - minValue) + (double) minValue);
    }

    public bool NextBool(double rate = 0.5)
    {
        return _random.NextDouble() > rate;
    }

    public string NextAlphaString(int length = 8)
    {
        return NextStringContaining(Alphabet, length);
    }

    public string NextNationalId()
    {
        return NextStringContaining(Digits, 10);
    }

    public string NextPhoneNumber()
    {
        return $"09{NextStringContaining(Digits, 9)}";
    }

    public string NextZipcode()
    {
        return NextStringContaining(Digits, 10);
    }

    public DateTime NextBirthDateTime()
    {
        return NextDateTimeInRange(BirthDateTime);
    }

    public DateTime NextEventDateTime()
    {
        return NextDateTimeInRange(EventDateTime);
    }

    public DateOnly NextEventDateOnly()
    {
        var year = _random.Next(EventDateTime.Item1.Year, EventDateTime.Item2.Year);
        var month = _random.Next(1, 13);
        var day = _random.Next(1, 29);
        return new DateOnly(year, month, day);
    }

    public TimeOnly NextTimeOnly()
    {
        return new TimeOnly(_random.Next(1, 23), _random.Next(1, 59));
    }

    public TEnum Pick<TEnum>() where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();
        return Pick(values);
    }

    public TEnum[] Pick<TEnum>(int length) where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();
        return NextArrayContaining(values, length);
    }

    public T Pick<T>(IList<T> list)
    {
        var length = list.Count;
        return list[_random.Next(length)];
    }
    
    public T Pick<T>(T[] array)
    {
        var length = array.Length;
        return array[_random.Next(length)];
    }

    private T[] NextArrayContaining<T>(IReadOnlyList<T> values, int length)
    {
        var valuesCount = values.Count;
        return Enumerable.Range(1, length)
            .Select(_ => values[_random.Next(valuesCount)])
            .ToArray();
    }

    private string NextStringContaining(string chars, int length)
    {
        var charsCount = chars.Length;
        return string.Join("",
            Enumerable.Range(1, length)
                .Select(_ => chars[_random.Next(charsCount)])
        );
    }

    private DateTime NextDateTimeInRange((DateTime, DateTime) range)
    {
        var timeSpan = range.Item2 - range.Item1;
        var randomTimeSpan = new TimeSpan((long) (_random.NextDouble() * timeSpan.Ticks));
        var randomDateTime = range.Item1 + randomTimeSpan;
        return randomDateTime;
    }
}