using Pos.Domain.Contracts.Entities;

namespace Pos.Domain.Entities;

public sealed class User : TenantEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
}