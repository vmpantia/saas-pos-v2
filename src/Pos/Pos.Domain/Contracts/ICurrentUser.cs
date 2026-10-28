namespace Pos.Domain.Contracts;

public interface ICurrentUser
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    string? Username { get; }
}
