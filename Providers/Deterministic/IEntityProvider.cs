namespace ResidenceMocker.Providers.Deterministic;

public interface IEntityProvider<out T>
{
    T Provide();
}