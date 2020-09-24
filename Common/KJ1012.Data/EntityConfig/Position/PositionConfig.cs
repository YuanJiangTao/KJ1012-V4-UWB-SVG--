using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KJ1012.Data.EntityConfig.Position
{
    public class PositionConfig : BaseEntityTypeConfiguration<Entities.Position.Position>
    {
        public override void Configure(EntityTypeBuilder<Entities.Position.Position> builder)
        {
            builder.ToTable(Entities.Position.Position.TableName);
            
            builder.HasIndex(e => new { e.TerminalId, e.PositionTime })
                .HasName("Pos_TerminalId_Position")
                .IsClustered();
            
            builder.Property(e => e.PositionTime).HasColumnType("datetime");
            base.Configure(builder);
            builder.HasKey(e => e.Id)
                .IsClustered(false);
        }
    }
}
