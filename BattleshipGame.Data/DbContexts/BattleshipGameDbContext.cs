using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.Data.DbContexts
{
    public class BattleshipGameDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;

        public DbSet<Field> Fields { get; set; } = null!;

        public BattleshipGameDbContext(DbContextOptions<BattleshipGameDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasData(
                new Player("Player1", "City1")
                {
                    Id = 1,
                },
                new Player("Player2", "City1")
                {
                    Id = 2,
                },
                new Player("Player3", "City1")
                {
                    Id = 3,
                },
                new Player("Player4", "City1")
                {
                    Id = 4,
                },
                new Player("Player5", "City1")
                {
                    Id = 5,
                });
                    

            base.OnModelCreating(modelBuilder);
        }
    }
}
