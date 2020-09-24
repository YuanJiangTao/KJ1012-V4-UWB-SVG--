using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Entities.Base;

namespace KJ1012.Data.EntityConfig.Base
{
    public class MemberConfig : BaseEntityTypeConfiguration<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable(Member.TableName);

            builder.Property(e => e.Address).HasMaxLength(100);

            builder.Property(e => e.BloodType).HasMaxLength(4);

            builder.Property(e => e.CardId).HasMaxLength(20);

            builder.Property(e => e.ContactPhone).HasMaxLength(30);

            builder.Property(e => e.DateOfBirth).HasColumnType("date");

            builder.Property(e => e.EntryDate).HasColumnType("date");

            builder.Property(e => e.FirstJobDate).HasColumnType("date");

            builder.Property(e => e.GraduateSchool).HasMaxLength(50);

            builder.Property(e => e.MemberNum).HasMaxLength(20);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PhoneNum).HasMaxLength(30);

            builder.Property(e => e.PhotoUrl).HasMaxLength(100);

            builder.Property(e => e.Sex).HasMaxLength(2);

            builder.Property(e => e.WorkAddress).HasMaxLength(50);

            builder.Property(e => e.AttModel).HasDefaultValueSql("((3))");

            builder.Property(e => e.MaxMinutes).HasDefaultValueSql("((480))");

            base.Configure(builder);
        }
    }
}
