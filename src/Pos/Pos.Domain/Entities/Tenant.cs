namespace Pos.Domain.Entities;

public sealed class Tenant : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
}