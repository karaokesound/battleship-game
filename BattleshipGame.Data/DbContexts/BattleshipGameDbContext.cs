using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.Data.DbContexts
{
    public class BattleshipGameDbContext : DbContext
    {
        public DbSet<PlayerEntity> Players { get; set; } = null!;

        public DbSet<FieldEntity> Fields { get; set; } = null!;

        public BattleshipGameDbContext(DbContextOptions<BattleshipGameDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerEntity>().HasData(
                new PlayerEntity("Player1", "City1")
                {
                    Id = 1,
                },
                new PlayerEntity("Player2", "City1")
                {
                    Id = 2,
                },
                new PlayerEntity("Player3", "City1")
                {
                    Id = 3,
                },
                new PlayerEntity("Player4", "City1")
                {
                    Id = 4,
                },
                new PlayerEntity("Player5", "City1")
                {
                    Id = 5,
                });
                    

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerEntity>().HasData(
                new PlayerEntity("Player1", "City1")
                {
                    Id = 1
                },
                new PlayerEntity("Player2", "City2")
                {
                    Id = 2
                },
                new PlayerEntity("Player3", "City3")
                {
                    Id = 3
                },
                new PlayerEntity("Player4", "City4")
                {
                    Id = 4
                },
                new PlayerEntity("Player5", "City5")
                {
                    Id = 5
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
