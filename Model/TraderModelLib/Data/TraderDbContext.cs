using System;
using Microsoft.EntityFrameworkCore;
using TraderModelLib.Models;

namespace TraderModelLib.Data
{
    public class TraderDbContext : DbContext
    {
        public static int currentId = 100;

        public static string ConnectionString { private get; set; }

        public TraderDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Trader> Traders { get; set; }
        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
        public DbSet<TraderToCurrency> T2Cs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trader>().HasData(
                new { Id = 0, FirstName = "Moshe", LastName = "Cohen", Birthdate = new DateTime(1991, 1, 1), Email = "mcohen@trader.com", IsDeleted = false, Password = "mmm" },
                new { Id = 1, FirstName = "Vasya", LastName = "Pupkin", Birthdate = new DateTime(1990, 1, 1),  Email = "vpupkin@trader.com".ToLower(), IsDeleted = false, Password = "vvv" }
            );

            modelBuilder.Entity<Cryptocurrency>().HasData(
             new { Id = 0, Currency = "Bitcoin",  Symbol = "XBT" },
             new { Id = 1, Currency = "Litecoin", Symbol = "LTC" },
             new { Id = 2, Currency = "Namecoin", Symbol = "NMC" }
            );

            modelBuilder.Entity<TraderToCurrency>().HasData(
             new { Id = 0, TraderId = 0, CurrencyId = 0 },
             new { Id = 1, TraderId = 0, CurrencyId = 1 },
             new { Id = 2, TraderId = 0, CurrencyId = 2 },
             new { Id = 3, TraderId = 1, CurrencyId = 0 },
             new { Id = 4, TraderId = 1, CurrencyId = 1 },
             new { Id = 5, TraderId = 1, CurrencyId = 2 }
            );
        }
    }
}
