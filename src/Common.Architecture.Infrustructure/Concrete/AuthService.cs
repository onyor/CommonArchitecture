using AutoMapper;
using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Utilities.Results;
using Common.Architecture.Core.Utilities.Security.Hashing;
using Common.Architecture.Infrastructure.Abstract;
using Common.Architecture.Persistance;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Shared.TransferObjects.Idendity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Architecture.Infrastructure.Concrete
{
    public class AuthService : IAuthService
    {

        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IUserService _userService;
        CommonDBContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthService(CommonDBContext context,IUserDal userDal, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IUnitOfWork unitOfWork, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = new CommonDBContext();
            _userService = userService;
        }


        //public async Task UpdateCurrentRoleAsync(Guid userId, Guid roleId)
        //{
        //    var user = await _context.Users.FindAsync(userId);
        //    user.RoleId = roleId;
        //    user.ModifiedAt = DateTime.Now;
        //    user.ModifiedBy = Guid.Parse(_currentUserService.GetUserId());

        //    await _context.SaveChangesAsync();
        //}

        public async Task<IDataResult<UserResponseDto>> LoginAsync(UserForLoginDto userForLoginDto)
        {

            var  userToCheck = await _userService.GetByEmailAsync(userForLoginDto.Email);

            if (!userToCheck.Success)
            {
                return new ErrorDataResult<UserResponseDto>("Sistemde böyle bir mail bulunamadı");
            }

            var resultUserLogin = await _signInManager.CheckPasswordSignInAsync(_mapper.Map<User>(userToCheck), userForLoginDto.Password, false);
            if (!resultUserLogin.Succeeded)
            {
                return new ErrorDataResult<UserResponseDto>("EPosta / Şifre hatalı giriş yapıldı!");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<UserResponseDto>("Hatalı Şifre");
            }

            if (!userToCheck.Data.UserRoles.Any())
            {
                //user does not have any valid roles
                return new ErrorDataResult<UserResponseDto>("Kullanıcıya ait geçerli bir rol bulunamadı!");
            }

            return new SuccessDataResult<UserResponseDto>(userToCheck.Data, "Sisteme başarı ile giriş yapıldı!");
        }

        public Task<IDataResult<UserForLoginDto>> BuildUserDtoAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
