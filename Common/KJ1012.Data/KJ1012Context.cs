using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using KJ1012.Data.EntityConfig;
using KJ1012.Data.ViewConfig;

namespace KJ1012.Data
{
    public class Kj1012Context:DbContext
    {
        public Kj1012Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(p => p.BaseType != null && p.BaseType.IsGenericType &&
                            p.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>));
            foreach (var type in types)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            var queryTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(p => p.BaseType != null && p.BaseType.IsGenericType &&
                            p.BaseType.GetGenericTypeDefinition() == typeof(BaseQueryTypeConfiguration<>));
            foreach (var type in queryTypes)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }

    }
}
