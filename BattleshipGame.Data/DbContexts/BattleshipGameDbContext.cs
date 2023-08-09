using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.Data.DbContexts
{
    public class BattleshipGameDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;

        public BattleshipGameDbContext(DbContextOptions<BattleshipGameDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasData(
                new Player("karaokesound", "Gdynia Obluze")
                {
                    Id = 1
                },

                new Player("nosia789", "Gdynia Obluze")
                {
                    Id = 2
                },

                new Player("pariparuva", "Gdynia Pogorze")
                {
                    Id = 3
                },

                new Player("ostrorzne", "Gdynia Dzialki Lesne")
                {
                    Id = 4
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
