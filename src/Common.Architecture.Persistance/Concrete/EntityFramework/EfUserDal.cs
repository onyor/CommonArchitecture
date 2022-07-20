using Common.Architecture.Core.DataAccess.EntityFramework;
using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Persistance.Abstract;

namespace Common.Architecture.Persistance.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, CommonDBContext>, IUserDal
    {
        
    }
}
