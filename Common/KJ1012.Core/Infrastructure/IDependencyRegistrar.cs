using Microsoft.Extensions.DependencyInjection;

namespace KJ1012.Core.Infrastructure
{
    /// <summary>
    /// 依赖性注册接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 注册服务和接口
        /// </summary>
        /// <param name="serviceCollection">提供服务集合对象</param>
        /// <param name="typeFinder">类型查找器</param>
        void Register(IServiceCollection serviceCollection, ITypeFinder typeFinder);
        /// <summary>
        /// 依赖注入排序号
        /// </summary>
        int Order { get; }
    }
}
