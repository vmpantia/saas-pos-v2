namespace Pos.Domain.Results.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials() => Error.Unauthorized("Auth.InvalidCredentials", "Invalid tenant code, username or password.");
    public static Error MissingSession() => Error.Unauthorized("Auth.MissingSession", "Your session is missing tenant or user information. Please sign in again.");
}
