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
        }
    }
}
