using AutoMapper;

namespace KJ1012.Core.Mapper
{
    /// <summary>
    /// AutoMapper 配置
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper 配置
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        /// <summary>
        /// 初始化 mapper
        /// </summary>
        /// <param name="config">Mapper 配置</param>
        public static void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();

        }
    }
}
