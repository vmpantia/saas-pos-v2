namespace Pos.Domain.Results.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials() => Error.Unauthorized("Auth.InvalidCredentials", "Invalid tenant code, username or password.");
}
