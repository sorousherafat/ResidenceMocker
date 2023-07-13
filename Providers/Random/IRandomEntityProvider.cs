namespace ResidenceMocker.Providers.Random;

public interface IRandomEntityProvider<out T>
{
    T Provide();
}