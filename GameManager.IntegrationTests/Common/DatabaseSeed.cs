using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using GameManager.Data;
using GameManager.Data.Enums;
using GameManager.Data.Models;

namespace GameManager.IntegrationTests.Common
{
    public class IntegrationTestDatabaseSeed
    {
        public const string AdminUser = "admin";

        public const string AdminPassword = "jnyD9cXUuuJmMF3M";

        public static async Task SeedAsync(GameManagerContext context)
        {
            try
            {
                await context.FriendSet.AddRangeAsync(GetFriends());
                await context.GameMediaSet.AddRangeAsync(GetGameMedias());
                await context.UserSet.AddRangeAsync(GetUsers());
                await context.UserHasRoleSet.AddRangeAsync(GetUserHasRoles());


                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static IEnumerable<Friend> GetFriends()
        {
            return new List<Friend>()
            {
                new Friend() {Id = 1, Active = 1, Name = "Finn", Email = "finn@ooo.com", Telephone = "9123"},
                new Friend() {Id = 2, Active = 1, Name = "Jake", Email = "jake@ooo.com", Telephone = "9123"},
                new Friend() {Id = 3, Active = 1, Name = "Bmo", Email = "bmo@ooo.com", Telephone = "9123"},
                new Friend() {Id = 4, Active = 1, Name = "Ice King", Email = "iceking@ooo.com", Telephone = "9123"},
            };
        }

        public static IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Active = 1,
                    Login = "admin",
                    Password = "k9aN7050+nC2VIpcd/y/RifG0gFVoosCR7H2IUgO6IdHAFzGYUNqHzqVGvBy7r2vbs2IHrngPWcLwMvFJNRFqg==",
                    Salt = "YAjG296GJMKTK8JiMuqa79A5N5Ralg4WprOyDglexN/KlWbbEkwVe8OYuhllJtNFnTRzg69pT6ilavOIa3gYcxUfdXXQazAlzv0IQoAojwqP2IoUfA5fwaN1/fQQIJ4FX7eFbJak0l6EQKmQY3v25fJSxjCiee7lnIRs4wTfVAw="
                }
            };
        }

        static IEnumerable<UserHasRole> GetUserHasRoles()
        {
            return new List<UserHasRole>()
            {
                new UserHasRole()
                {
                    Id =1,
                    Active = 1,
                    Role = "ADMIN",
                    UserId = 1
                }
            };
        }

        public static IEnumerable<GameMedia> GetGameMedias()
        {
            return new List<GameMedia>()
            {
                new GameMedia() {Id=1, Active = 1, Title = "Super Mario World", Year = 1995, MediaType = MediaType.Cartridge, Platform = Platform.SNES},
                new GameMedia() {Id=2, Active = 1, Title = "Crash", Year = 1996, MediaType = MediaType.CD, Platform = Platform.PS},
                new GameMedia() {Id=3, Active = 1, Title = "Command & Conquer", Year = 1995, MediaType = MediaType.CD, Platform = Platform.PC},
                new GameMedia() {Id=4, Active = 1, Title = "Age of Empires", Year = 1997, MediaType = MediaType.CD, Platform = Platform.PC},
                new GameMedia() {Id=5, Active = 1, Title = "Sim City", Year = 1989, MediaType = MediaType.Cartridge, Platform = Platform.SNES, BorrowerId = 2},
            };
        }
    }
}
