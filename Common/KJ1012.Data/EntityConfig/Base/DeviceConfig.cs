using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Entities.Base;

namespace KJ1012.Data.EntityConfig.Base
{
    public class DeviceConfig : BaseEntityTypeConfiguration<Device>
    {
        public override void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable(Device.TableName);
            builder.Property(e => e.AddressDescribe).HasMaxLength(50);
            
            builder.Property(e => e.Describe).HasMaxLength(50);

            builder.Property(e => e.DeviceName).HasMaxLength(50);

            builder.Property(e => e.Extend1).HasMaxLength(50);

            builder.Property(e => e.Extend2).HasMaxLength(50);

            builder.Property(e => e.Extend3).HasMaxLength(50);

            builder.Property(e => e.Extend4).HasMaxLength(50);

            builder.Property(e => e.Extend5).HasMaxLength(50);

            builder.Property(e => e.LastCheckTime).HasColumnType("datetime");

            builder.HasOne(d => d.Substation)
                .WithMany(p => p.Substations)
                .HasForeignKey(d => d.SubstationId)
                .HasConstraintName("FK_Base_Device_Base_Device1");
            base.Configure(builder);
        }
    }
}
