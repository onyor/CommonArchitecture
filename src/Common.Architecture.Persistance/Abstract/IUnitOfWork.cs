using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Persistance.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserDal Users { get; }

        Task<int> SaveAsync();
    }
}
