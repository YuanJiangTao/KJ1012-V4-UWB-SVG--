using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Entities;

namespace KJ1012.Data.ViewConfig
{
    public abstract class BaseQueryTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseView 
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasNoKey();
        }
    }
}
