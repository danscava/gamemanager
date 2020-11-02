using System;
using System.Collections.Generic;
using System.Text;
using GameManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.Data
{
    public class GameManagerContext : DbContext
    {
        public GameManagerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> UserSet { get; set; }

        public DbSet<UserHasRole> UserHasRoleSet { get; set; }

        public DbSet<GameMedia> GameMediaSet { get; set; }

        public DbSet<Friend> FriendSet { get; set; }

    }

}
