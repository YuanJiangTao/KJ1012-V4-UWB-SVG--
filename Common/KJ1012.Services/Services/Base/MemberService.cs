using System.Threading.Tasks;
using KJ1012.Core.Data;
using KJ1012.Data.Entities.Base;
using KJ1012.Data.Views;
using KJ1012.Services.IServices.Base;
using Microsoft.EntityFrameworkCore;

namespace KJ1012.Services.Services.Base
{
    public class MemberService : BaseService<Member>, IMemberService
    {
        public MemberService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


    }
}
