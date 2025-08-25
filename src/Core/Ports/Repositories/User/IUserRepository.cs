namespace Core.Ports.Repositories.User;

public interface IUserRepository
{
    Task<Entities.User?> GetByUsernameAsync(string username);
    Task<Entities.User?> GetByIdAsync(Guid id);
    Task AddAsync(Entities.User user);
    Task<bool> ExistsByEmailAsync(string email);
}