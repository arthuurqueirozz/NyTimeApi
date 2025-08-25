namespace Core.Ports.Auth;

public interface IAuthService
{
    Task<Entities.User> RegisterAsync(string username, string email, string passwordHash);
    Task<Entities.User?> ValidateUserAsync(string username, string passwordHash);
}