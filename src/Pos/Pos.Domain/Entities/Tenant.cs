using Pos.Domain.Contracts.Entities;

namespace Pos.Domain.Entities;

public sealed class Tenant : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
}