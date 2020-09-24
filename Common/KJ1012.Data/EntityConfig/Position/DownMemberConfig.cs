using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Entities.Position;

namespace KJ1012.Data.EntityConfig.Position
{
    public class DownMemberConfig : BaseEntityTypeConfiguration<DownMember>
    {
        public override void Configure(EntityTypeBuilder<DownMember> builder)
        {
            builder.ToTable(DownMember.TableName);

            builder.HasKey(e => e.TerminalId)
                .IsClustered(false);

            builder.HasIndex(e => new { e.CreateDate, e.TerminalId })
                .HasName("IX_Pos_DownMember")
                .IsClustered();

            builder.Property(e => e.TerminalId).ValueGeneratedNever();

            builder.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");

            builder.Property(e => e.PositionTime).HasColumnType("datetime");
        }
    }
}
