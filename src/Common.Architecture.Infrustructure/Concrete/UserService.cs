using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Entities.ViewModels;
using Common.Architecture.Core.Utilities.Results;
using Common.Architecture.Infrastructure.Abstract;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Shared.TransferObjects.Idendity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Infrastructure.Concrete
{
    public class UserService : IUserService
    {
        IUserDal _userDal;
        private readonly UserManager<User> _userManager;

        public UserService(IUserDal userDal, UserManager<User> userManager)
        {
            _userManager = userManager;
            _userDal = userDal;
        }

        public Task<IDataResult<UserDto>> AddAsync(UserDto dto, string password, string rolAd)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                return new ErrorResult();
            }
            else
            {
                _userDal.AddAsync(user);

                return new SuccessResult();
            }
        }

        public Task<IResult> DeleteAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IReadOnlyList<UserDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<UserDto>> GetByIdAsync(Guid id, bool isDeleted = false)
        {
            var result = _userDal.Get(u => u.Email == email);

            if (result != null)
            {
                return new SuccessDataResult<User>(result);
            }
            return new ErrorDataResult<User>();
        }

        public Task<IDataResult<JsonResult>> LoadDataTableAsync(DataTableViewModel vm, bool isActive = true, bool isDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<UserDto>> UpdateAsync(UserDto dto, string password)
        {
            if (user != null)
            {
                _userDal.Update(user);

                return new SuccessResult();
            }

            return new ErrorResult();
        }
    }
}
