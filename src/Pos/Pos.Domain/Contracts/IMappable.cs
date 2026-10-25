namespace Pos.Domain.Contracts;

public interface IMappable<out TDestination>
{
    TDestination Map();
}