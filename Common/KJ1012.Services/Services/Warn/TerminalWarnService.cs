using KJ1012.Core.Data;
using KJ1012.Data.Entities.Warn;
using KJ1012.Domain.Enums;
using KJ1012.Services.IServices.Warn;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KJ1012.Services.Services.Warn
{
    public class TerminalWarnService : BaseService<TerminalWarn>, ITerminalWarnService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TerminalWarnService(
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<int> SaveAsync(TerminalWarn entity, bool alwaysUpdate = false)
        {
            if (entity.TerminalState == (int)TerminalStateEnum.TerminalStateOk)
            {
                var entities = BaseRepository.Table.Where(r => r.TerminalId == entity.TerminalId && !r.RecoveryTime.HasValue);
                foreach (var terminalWarn in entities)
                {
                    terminalWarn.RecoveryTime = DateTime.Now;
                    terminalWarn.RecoveryType = 0;
                }
                entity.RecoveryTime = DateTime.Now;
                entity.RecoveryType = 0;
                return await _unitOfWork.SaveChangesAsync();
            }

            var exitsEntity = await BaseRepository.Table.FirstOrDefaultAsync(r => r.TerminalId == entity.TerminalId
                                    && !r.RecoveryTime.HasValue);
            if (exitsEntity == null)
            {
                return await base.SaveAsync(entity);
            }
            if (exitsEntity.TerminalState != entity.TerminalState)
            {
                exitsEntity.RecoveryTime = DateTime.Now;
                exitsEntity.RecoveryType = 0;
                exitsEntity.RecoveryRemark = "自动识别恢复";
                await BaseRepository.InsertAsync(entity);
            }
            return await _unitOfWork.SaveChangesAsync();
        }

    }
}
