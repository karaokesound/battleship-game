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
    }
}
