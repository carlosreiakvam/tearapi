using Microsoft.EntityFrameworkCore;

namespace TearApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Opposite> Opposites { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure any additional model settings or relationships here
            // For example, configuring the foreign key relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Opposite)
                .WithMany()
                .HasForeignKey(u => u.OppositeId);

            // Seed initial data
            //SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            /*           // Add initial Opposite data
                       modelBuilder.Entity<Opposite>().HasData(
                           new Opposite { OppositeId = -1, Description = "Opposite 1", Intensity = 0 },
                           new Opposite { OppositeId = -2, Description = "Opposite 2", Intensity = 0 },
                           new Opposite { OppositeId = -3, Description = "Opposite 3", Intensity = 1 }
                       );

                       // Add initial User data
                       modelBuilder.Entity<User>().HsasData(
                           new User { UserId = -1, OppositeId = -1, IsAgreeing = true },
                           new User { UserId = -2, OppositeId = -1, IsAgreeing = true },
                           new User { UserId = -3, OppositeId = -2, IsAgreeing = true }
                       );
                   }*/
        }
    }
}
