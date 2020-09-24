using System.Threading.Tasks;
using KJ1012.Data.Views;

namespace KJ1012.Services.IServices.View
{
    public interface IViewTerminalService : IBaseViewService<ViewTerminal>
    {
        Task<ViewTerminal> GetEntityAsync(int terminal);
    }
}