namespace Core
{
    public interface IAggregate<out TIdentifier>
    {
        TIdentifier Id { get; }
    }
}
