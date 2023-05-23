using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pri.Ca.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Infrastructure.Data
{
    public static class Seeder
    {
        

        public static void Seed(ModelBuilder modelBuilder)
        {
            PasswordHasher<ApplicationUser> passwordHasher = new();
            //create an admin user
            var admin = new ApplicationUser 
            { 
                Id = "666",
                UserName = "mileto@games.com",
                Firstname = "Mileto",
                Lastname = "Di Marco",
                Email = "mileto@games.com",
                DateOfBirth = new DateTime(1975,02,19),
                NormalizedEmail = "MILETO@GAMES.COM",
                NormalizedUserName = "MILETO@GAMES.COM",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            //hash the password
            admin.PasswordHash = passwordHasher.HashPassword(admin, "123");//never use this password in real life :)
            //add to seeding
            modelBuilder.Entity<ApplicationUser>()
                .HasData(admin);
            //add claims for admin
            IdentityUserClaim<string>[] identityUserClaims = new
            IdentityUserClaim<string>[]
            {
                new IdentityUserClaim<string> {Id = 101,ClaimType = ClaimTypes.Role,ClaimValue="Admin",UserId="666" },
                new IdentityUserClaim<string> {Id = 102,ClaimType = ClaimTypes.DateOfBirth,ClaimValue= admin.DateOfBirth.ToString(),UserId="666" },
            };
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(identityUserClaims);

            //create a user
            var underAgeUser = new ApplicationUser
            {
                Id = "667",
                UserName = "luca@games.com",
                Firstname = "Luca",
                Lastname = "Di Marco",
                Email = "luca@games.com",
                DateOfBirth = new DateTime(2010, 02, 19),
                NormalizedEmail = "LUCA@GAMES.COM",
                NormalizedUserName = "LUCA@GAMES.COM",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            //hash the password
            underAgeUser.PasswordHash = passwordHasher.HashPassword(underAgeUser, "123");//never use this password in real life :)
            //add to seeding
            modelBuilder.Entity<ApplicationUser>()
                .HasData(underAgeUser);
            //add claims for user
            identityUserClaims = new
            IdentityUserClaim<string>[]
            {
                new IdentityUserClaim<string> {Id = 103,ClaimType = ClaimTypes.Role,ClaimValue="User",UserId="667" },
                new IdentityUserClaim<string> {Id = 104,ClaimType = ClaimTypes.DateOfBirth,ClaimValue= underAgeUser.DateOfBirth.ToString(),UserId="667" },
            };
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(identityUserClaims);
            
            //Games
            var games = new Game[]
            {
                new Game{ Id = 1,Name="Fifa2000"},
                new Game{ Id = 2,Name="Wolfenstein"},
                new Game{ Id = 3,Name="Minecraft"}
            };
            //categories
            var categories = new Category[]
            {
                new Category { Id = 1, Name = "Sports" },
                new Category { Id = 2, Name = "Action" },
                new Category { Id = 3, Name = "Kids" }
            };
            //CategoryGame
            var categoryGames = new[]
            {
                new {GamesId=1,CategoriesId=1 },
                new {GamesId=1,CategoriesId=2 },
                new {GamesId=1,CategoriesId=3 },
                new {GamesId=2,CategoriesId=1 },
                new {GamesId=2,CategoriesId=3 },
                new {GamesId=2,CategoriesId=2 },
                new {GamesId=3,CategoriesId=1 },
                new {GamesId=3,CategoriesId=2 },
            };
            //sale
            var sales = new Sale[]
            {
                new Sale {Id = 1, GameId = 1, Quantity = 1 },
                new Sale {Id = 2, GameId = 2, Quantity = 10 },
                new Sale {Id = 3, GameId = 3, Quantity = 2 },
                new Sale {Id = 4, GameId = 1, Quantity = 8 },
                new Sale {Id = 5, GameId = 2, Quantity = 12 },
                new Sale {Id = 6, GameId = 3, Quantity = 4 },
                new Sale {Id = 7, GameId = 1, Quantity = 10 },
                new Sale {Id = 8, GameId = 2, Quantity = 1 },
                new Sale {Id = 9, GameId = 3, Quantity = 5 },
                new Sale {Id = 10,GameId = 1, Quantity = 13 },
                new Sale {Id = 12,GameId = 2, Quantity = 1 },
                new Sale {Id = 13,GameId = 3, Quantity = 4 },
                new Sale {Id = 14,GameId = 1, Quantity = 8 },
                new Sale {Id = 15,GameId = 2, Quantity = 4 },
                new Sale {Id = 16,GameId = 3, Quantity = 23 },
                new Sale {Id = 17, GameId = 1, Quantity = 2 },
                new Sale {Id = 18, GameId = 2, Quantity = 7 },
                new Sale {Id = 19, GameId = 3, Quantity = 9 },
                new Sale {Id = 20, GameId = 1, Quantity = 10 },
                new Sale {Id = 21, GameId = 2, Quantity = 2 },
                new Sale {Id = 22, GameId = 3, Quantity = 5 },
                new Sale {Id = 23, GameId = 1, Quantity = 9 },
                new Sale {Id = 24, GameId = 2, Quantity = 8 },
                new Sale {Id = 25, GameId = 3, Quantity = 10 },
            };
            //hasdata methods
            modelBuilder.Entity<Game>().HasData(games);
            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity($"{nameof(Category)}{nameof(Game)}").HasData(categoryGames);
            modelBuilder.Entity<Sale>().HasData(sales);
        }
    }
}
