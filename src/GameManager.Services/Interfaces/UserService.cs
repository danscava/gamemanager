using System;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using Microsoft.Extensions.Logging;

namespace GameManager.Services.Interfaces
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IAsyncUserRepository _userRepository;

        public UserService(ILogger<UserService> logger, IAsyncUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password,
                Convert.FromBase64String(user.Password),
                Convert.FromBase64String(user.Salt)))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Empty password not allowed");
            }

            if (await _userRepository.GetByUsernameAsync(user.Login) != null)
            {
                throw new Exception("User already exists");
            }

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.Password = Convert.ToBase64String(passwordHash);
            user.Salt = Convert.ToBase64String(passwordSalt);

            await _userRepository.AddAsync(user);

            return user;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                return true;
            }

            return false;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
