using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Persistance.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CommonDBContext _context;

        private EfUserDal _userDal;
        public UnitOfWork(CommonDBContext context)
        {
            _context = context;
        }
        public IUserDal Users => _userDal ?? new EfUserDal();

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
