using System.Threading.Tasks;
using KJ1012.Data.Entities.Base;

namespace KJ1012.Services.Publish
{
    public interface IDevicePublish:IPublish
    {
        Task AddPublishAsync(Device device);
        Task DeletePublishAsync(Device device);
    }
}
