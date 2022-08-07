using Common.Architecture.Core.Entities.ViewModels;
using Common.Architecture.Core.Utilities.Results;
using Common.Architecture.Shared.TransferObjects.Idendity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Infrastructure.Abstract
{
    public interface IUserService
    {
        Task<IResult> AddAsync(UserForLoginDto dto, string password, string rolAd);
        Task<IResult> UpdateAsync(UserForLoginDto dto, string password);
        Task<IResult> DeleteAsync (Guid userId);
        Task<IDataResult<JsonResult>> LoadDataTableAsync(DataTableViewModel vm, bool isActive = true, bool isDeleted = false);
        Task<IDataResult<UserForLoginDto>> GetByIdAsync(Guid id, bool isDeleted = false);
        Task<IDataResult<List<UserForLoginDto>>> GetAllAsync();

        //IDataResult<List<OperationClaim>> GetClaims(User user);
        //IResult Add(User user);
        //IDataResult<User> GetByMail(string email);
    }
}
