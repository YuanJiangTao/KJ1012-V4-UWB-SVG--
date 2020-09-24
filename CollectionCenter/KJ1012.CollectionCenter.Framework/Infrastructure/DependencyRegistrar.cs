using System.Linq;
using KJ1012.Core.Data;
using KJ1012.Core.Infrastructure;
using KJ1012.Data;
using KJ1012.Services;
using KJ1012.Services.Publish;
using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.CollectionCenter.Framework.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder)
        {
            #region 注册数据服务对象

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));

            var baseServices = typeFinder.FindClassesOfType(typeof(IDataService));
            foreach (var baseService in baseServices)
            {
                services.AddScoped(baseService.GetInterfaces().Last(), baseService);
            }

            var publishServices = typeFinder.FindClassesOfType(typeof(IPublish));
            foreach (var baseService in publishServices)
            {
                services.AddScoped(baseService.GetInterfaces().First(), baseService);
            }

            #endregion
        }


        public int Order => 1;
    }
}
