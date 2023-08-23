using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BattleshipGame.Data.Configuration
{
    public class FieldConfiguration : IEntityTypeConfiguration<FieldEntity>
    {
        public void Configure(EntityTypeBuilder<FieldEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.X)
                .IsRequired();

            builder.Property(f => f.Y)
                .IsRequired();

            builder.Property(f => f.ShipSize)
                .IsRequired();

            builder.Property(f => f.ShipId)
                .IsRequired();

            builder.Property(f => f.IsEmpty)
                .IsRequired();

            builder.Property(f => f.IsHitted)
                .IsRequired();

            builder.Property(f => f.IsValid)
                .IsRequired();

            // one-to-many relation
            builder.HasOne(p => p.Player)
                .WithMany(p => p.GameBoard)
                .HasForeignKey(p => p.PlayerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
