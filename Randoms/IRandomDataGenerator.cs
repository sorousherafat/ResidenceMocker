namespace ResidenceMocker.Randoms;

public interface IRandomDataGenerator
{
    int Next(int maxValue);
    
    int Next(int minValue, int maxValue);

    short NextShort(short minValue, short maxValue);

    double NextDouble(double minValue, double maxValue);

    decimal NextDecimal(decimal minValue, decimal maxValue);

    bool NextBool(double rate = 0.5);
    
    string NextAlphaString(int length = 8);

    string NextNationalId();

    string NextPhoneNumber();

    string NextZipcode();

    DateTime NextEventDateTime();

    DateOnly NextEventDateOnly();

    TimeOnly NextTimeOnly();

    TEnum Pick<TEnum>() where TEnum : struct, Enum;
    
    TEnum[] Pick<TEnum>(int length) where TEnum : struct, Enum;
    
    T Pick<T>(IList<T> list);
}