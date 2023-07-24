namespace ResidenceMocker.Mockers.Entity;

public interface IEntityMocker<out T>
{
    T MockEntity();
}