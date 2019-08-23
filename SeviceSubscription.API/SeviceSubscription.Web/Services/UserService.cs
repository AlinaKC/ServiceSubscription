using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Options;
using ServiceSubscription.Core;
using SeviceSubscription.Core.Model;

namespace SeviceSubscription.Web
{
    public interface IUserService
    {
        UserInfo Authenticate(string username, string password);
        UserInfo CreateUser(UserInfo user, string password);
    }

    public class UserService : IUserService
    {
        private List<UserInfo> _users = new List<UserInfo>();
        private IUserProvider _userProvider;


        public UserService(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public UserInfo Authenticate(string username, string password)
        {
            _users = _userProvider.GetAllUsers();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _users.SingleOrDefault(x => x.UserName == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public UserInfo CreateUser(UserInfo user, string password)
        {
            _users = _userProvider.GetAllUsers();
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");

            if (_users.Any(x => x.Email == user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            if (string.IsNullOrWhiteSpace(user.Role))
                user.Role = Role.User;
            _userProvider.AddUser(user);
            return user;
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}