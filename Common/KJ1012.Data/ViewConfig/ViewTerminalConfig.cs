using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KJ1012.Data.Views;

namespace KJ1012.Data.ViewConfig
{
    public class ViewTerminalConfig : BaseQueryTypeConfiguration<ViewTerminal>
    {
        public override void Configure(EntityTypeBuilder<ViewTerminal> builder)
        {
            builder.ToView(ViewTerminal.TableName);
            base.Configure(builder);
        }
    }
}
