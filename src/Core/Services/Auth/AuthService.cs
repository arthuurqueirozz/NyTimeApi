using Core.Ports.Auth;
using Core.Ports.Repositories.User;

namespace Core.Services.Auth
{
    public class AuthService :  IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Entities.User> RegisterAsync(string username, string email, string passwordHash)
        {
            var exists = await _userRepository.ExistsByEmailAsync(email);
            if (exists) throw new InvalidOperationException("Email already registered");

            var user = new Entities.User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<Entities.User?> ValidateUserAsync(string username, string passwordHash)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || user.PasswordHash != passwordHash)
                return null;

            return user;
        }
    }
}