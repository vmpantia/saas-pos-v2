using Pos.Domain.Entities;

namespace Pos.Domain.Contracts.Services;

public interface IPasswordHasher
{
    string Hash(User user, string password);
    bool Verify(User user, string password);
}
