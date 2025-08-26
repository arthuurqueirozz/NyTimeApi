using Core.Ports.Auth;
using Core.Ports.Repositories.User;

namespace Core.Services.Auth
{
    public class AuthService :  IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher; 

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasherService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasherService;
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

        public async Task<Entities.User?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            
            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}