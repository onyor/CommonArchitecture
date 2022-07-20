using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Persistance.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
    }
}
