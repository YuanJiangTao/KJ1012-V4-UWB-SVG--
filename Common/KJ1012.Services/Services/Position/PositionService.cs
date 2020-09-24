using KJ1012.Core.Data;
using KJ1012.Services.IServices.Position;

namespace KJ1012.Services.Services.Position
{
    public partial class PositionService : BaseService<Data.Entities.Position.Position>, IPositionService
    {

        public PositionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
