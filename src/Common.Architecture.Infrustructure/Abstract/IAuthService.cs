using Common.Architecture.Core.Utilities.Results;
using Common.Architecture.Shared.TransferObjects.Idendity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Infrastructure.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<UserForLoginDto>> BuildUserDtoAsync(Guid userId);
    }
}
