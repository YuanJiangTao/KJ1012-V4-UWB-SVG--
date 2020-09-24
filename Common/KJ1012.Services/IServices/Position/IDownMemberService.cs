using System.Threading.Tasks;
using KJ1012.Data.Entities.Position;

namespace KJ1012.Services.IServices.Position
{
    public interface IDownMemberService : IBaseService<DownMember>
    {
        Task<DownMember> GetEntityAsync(int terminalId);
        Task<int> UpdateAsync(DownMember entity, Data.Entities.Position.Position positionRecord);

    }
}