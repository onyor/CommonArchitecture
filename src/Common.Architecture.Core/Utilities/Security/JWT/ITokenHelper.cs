using Common.Architecture.Core.Entities.Concrete;
using System.Collections.Generic;
using System.Security.Claims;

namespace Common.Architecture.Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<ClaimsIdentity> identityClaims);
    }
}
