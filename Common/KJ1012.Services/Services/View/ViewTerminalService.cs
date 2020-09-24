using Microsoft.EntityFrameworkCore;
using KJ1012.Core.Data;
using KJ1012.Data.Views;
using KJ1012.Services.IServices.View;
using System.Threading.Tasks;

namespace KJ1012.Services.Services.View
{
    public class ViewTerminalService : BaseViewService<ViewTerminal>, IViewTerminalService
    {
        private readonly IViewRepository<ViewTerminal> _query;

        public ViewTerminalService(IViewRepository<ViewTerminal> query) : base(query)
        {
            _query = query;
        }

        public async Task<ViewTerminal> GetEntityAsync(int terminal)
        {
            return await _query.View.FirstOrDefaultAsync(f => f.TerminalId == terminal);
        }
    }
}