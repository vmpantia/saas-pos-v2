using Pos.Domain.Entities;

namespace Pos.Domain.Contracts;

public interface IPasswordHasher
{
    string Hash(User user, string password);
    bool Verify(User user, string password);
}
