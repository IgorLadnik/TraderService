using System;
using Microsoft.EntityFrameworkCore;
using TraderModelLib.Models;

namespace TraderModelLib.Data
{
    public class TraderDbContext : DbContext
    {
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
                new { Id = 1, FirstName = "Vasya", LastName = "Pupkin", Birthdate = new DateTime(1990, 1, 1),  Email = "vpupkin@trader.com".ToLower(), IsDeleted = false, Password = "vvv" },
                new { Id = 2, FirstName = "Moshe", LastName = "Levy", Birthdate = new DateTime(1991, 1, 1), Email = "mlevy@trader.com", IsDeleted = false, Password = "mmm" }
            );

            modelBuilder.Entity<Cryptocurrency>().HasData(
             new { Id = 1,  Currency = "Bitcoin", Symbol = "XBT" },
             new { Id = 2, Currency = "Litecoin", Symbol = "LTC" },
             new { Id = 3, Currency = "Namecoin", Symbol = "NMC" }
            );

            modelBuilder.Entity<TraderToCurrency>().HasData(
             new { Id = 1, TraderId = 1, CurrencyId = 1 },
             new { Id = 2, TraderId = 1, CurrencyId = 2 },
             new { Id = 3, TraderId = 1, CurrencyId = 3 },
             new { Id = 4, TraderId = 2, CurrencyId = 1 },
             new { Id = 5, TraderId = 2, CurrencyId = 2 },
             new { Id = 6, TraderId = 2, CurrencyId = 3 }
            );
        }
    }
}
