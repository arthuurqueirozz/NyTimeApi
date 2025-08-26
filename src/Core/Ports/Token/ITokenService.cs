using Core.Entities;

namespace Core.Ports.Token;

public interface ITokenService
{
    string GenerateToken(User user);
}