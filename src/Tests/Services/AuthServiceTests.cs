using Core.Entities;
using Core.Ports.Auth;
using Core.Ports.Repositories.User;
using Core.Services.Auth;
using FluentAssertions;
using Moq;

namespace Tests.Services;

public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
        {
            _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            Func<Task> act = () => _authService.RegisterAsync("testuser", "test@test.com", "passwordHash");

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Email already registered");
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnUser_WhenRegistrationIsSuccessful()
        {
            const string username = "newuser";
            const string email = "new@test.com";
            const string passwordHash = "hashedPassword123";
            _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(email)).ReturnsAsync(false);
            
            var result = await _authService.RegisterAsync(username, email, passwordHash);

            _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Username == username && u.Email == email)), Times.Once);
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
        }
        
        [Fact]
        public async Task ValidateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            var result = await _authService.ValidateUserAsync("nouser", "anypassword");

            result.Should().BeNull();
        }

        [Fact]
        public async Task ValidateUserAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            var user = new User { Username = "testuser", PasswordHash = "correctHash" };
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(user.Username)).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.VerifyPassword("wrongPassword", user.PasswordHash)).Returns(false);

            var result = await _authService.ValidateUserAsync(user.Username, "wrongPassword");

            result.Should().BeNull();
        }

        [Fact]
        public async Task ValidateUserAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var user = new User { Username = "testuser", PasswordHash = "correctHash" };
            var password = "correctPassword";
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(user.Username)).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.VerifyPassword(password, user.PasswordHash)).Returns(true);
            
            var result = await _authService.ValidateUserAsync(user.Username, password);

            result.Should().NotBeNull();
            result.Username.Should().Be(user.Username);
        }
    }