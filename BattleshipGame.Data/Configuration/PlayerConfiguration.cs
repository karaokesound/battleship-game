using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BattleshipGame.Data.Configuration
{
    public class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
    {
        public void Configure(EntityTypeBuilder<PlayerEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.City)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.Game);

            builder.Property(p => p.SunkenShips);

            builder.Property(p => p.CanShoot);

            // many-to-one relation
            builder.HasMany(g => g.GameBoard)
                .WithOne(g => g.Player)
                .HasForeignKey(g => g.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
