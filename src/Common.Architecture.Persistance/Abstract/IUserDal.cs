using Common.Architecture.Core.DataAccess.Interface;
using Common.Architecture.Core.Entities.Concrete;

namespace Common.Architecture.Persistance.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
    }
}
