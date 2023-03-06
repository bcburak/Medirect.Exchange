using MeDirect.Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeDirect.Exchange.Persistence.Context
{
    public class CurrencyExchangeDbContext : DbContext, IDisposable
    {

        protected readonly IConfiguration Configuration;

        public CurrencyExchangeDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                //options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Chinook");
                options.UseSqlServer(Configuration.GetConnectionString("MedirectDb"));
            }
            // connect to sql server with connection string from app settings
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Chinook");
        //    }
        //}
        public DbSet<Currency> Currency { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.TargetCurrency)
                .WithMany()
                .HasForeignKey(t => t.TargetCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "John", LastName = "Doe", Name = "johnDoe", Email = "john.doe@example.com", TradesPerHourLimit = 10 },
                new User { Id = 2, FirstName = "Jane", LastName = "Doe", Name = "janeDoe", Email = "jane.doe@example.com", TradesPerHourLimit = 10 },
                new User { Id = 3, FirstName = "Bob", LastName = "Smith", Name = "bobSmith", Email = "bob.smith@example.com", TradesPerHourLimit = 10 }
            );

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entity.UpdatedDate = now;
                            break;
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            entity.UpdatedDate = now;
                            break;
                    }
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
