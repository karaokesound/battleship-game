using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BattleshipGame.Data.Configuration
{
    public class GameConfiguration : IEntityTypeConfiguration<GameEntity>
    {
        public void Configure(EntityTypeBuilder<GameEntity> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .ValueGeneratedOnAdd();

            // Relation with players
            builder.HasOne(g => g.Player1)
                .WithMany()
                .IsRequired()
                .HasForeignKey(g => g.Player1.Id);

            builder.HasOne(g => g.Player2)
                .WithMany()
                .IsRequired()
                .HasForeignKey(g => g.Player2.Id);

            // Relation with players' boards
            builder.HasMany(g => g.Player1Board)
                .WithOne()
                .HasForeignKey(f => f.Id)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(g => g.Player2Board)
                .WithOne()
                .HasForeignKey(f => f.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
