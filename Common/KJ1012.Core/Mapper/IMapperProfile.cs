namespace KJ1012.Core.Mapper
{
    public interface IMapperProfile
    {
        /// <summary>
        /// 返回配置注入顺序
        /// </summary>
        int Order { get; }
    }
}
