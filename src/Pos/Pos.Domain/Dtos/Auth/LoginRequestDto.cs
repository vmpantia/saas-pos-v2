using System.ComponentModel.DataAnnotations;

namespace Pos.Domain.Dtos.Auth;

public sealed class LoginRequestDto
{
    [Required] public string TenantCode { get; set; } = string.Empty;
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}
