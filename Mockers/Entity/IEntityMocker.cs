namespace ResidenceMocker.Mockers;

public interface IEntityMocker<out T>
{
    T MockEntity(int id);
}