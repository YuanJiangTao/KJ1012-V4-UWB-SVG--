using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Entities.Warn;

namespace KJ1012.Data.EntityConfig.Warn
{
    public class TerminalWarnConfig : BaseEntityTypeConfiguration<TerminalWarn>
    {
        public override void Configure(EntityTypeBuilder<TerminalWarn> builder)
        {

            builder.ToTable(TerminalWarn.TableName);

            builder.HasIndex(e => new { e.TerminalId, e.CreateDate })
                .HasName("IX_TerminalId_CreateDate")
                .IsClustered();

            builder.HasIndex(e => new { e.TerminalId, e.RecoveryTime, e.TerminalState })
                .HasName("IX_TerminalId_ReTime_State")
                .IsUnique();

            builder.Property(e => e.RecoveryTime).HasColumnType("datetime");

            builder.Property(e => e.RecoveryRemark).HasMaxLength(50);

            base.Configure(builder);
        }
    }
}
