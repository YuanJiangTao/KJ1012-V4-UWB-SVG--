using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Entities.Warn;

namespace KJ1012.Data.EntityConfig.Warn
{
    public class DeviceWarnConfig : BaseEntityTypeConfiguration<DeviceWarn>
    {
        public override void Configure(EntityTypeBuilder<DeviceWarn> builder)
        {
            builder.ToTable(DeviceWarn.TableName);

            base.Configure(builder);

            builder.HasKey(e => e.Id)
                .IsClustered(false);

            builder.HasIndex(e => new { e.CreateDate, e.DeviceId })
                .HasName("DeviceId_CreateDateIndex")
                .IsClustered();

            builder.Property(e => e.RecoveryTime).HasColumnType("datetime");

            builder.Property(e => e.RecoveryRemark).HasMaxLength(50);

            builder.HasOne(d => d.Device)
                .WithMany(p => p.DeviceWarn)
                .HasForeignKey(d => d.DeviceId);

            builder.HasQueryFilter(f => f.DeviceState != 64);
        }
    }
}
