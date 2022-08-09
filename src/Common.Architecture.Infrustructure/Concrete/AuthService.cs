using AutoMapper;
using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Utilities.Results;
using Common.Architecture.Infrastructure.Abstract;
using Common.Architecture.Persistance;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Shared.TransferObjects.Idendity;
using Microsoft.AspNetCore.Identity;
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
        CommonDBContext _context;
        private readonly UserManager<User> _userManager;

        public AuthService(CommonDBContext context,IUserDal userDal, UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context= context;
        }

        //public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        //{
        //    byte[] passwordHash, passwordSalt;
        //    HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
        //    var user = new User
        //    {
        //        Email = userForRegisterDto.Email,
        //        FirstName = userForRegisterDto.FirstName,
        //        LastName = userForRegisterDto.LastName,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        Status = true
        //    };
        //    _userService.Add(user);

        //    return new SuccessDataResult<User>(user, Messages.UserRegistered);
        //}

        //public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        //{
        //    var userToCheck = _userService.GetByMail(userForLoginDto.Email);
        //    if (userToCheck == null)
        //    {
        //        return new ErrorDataResult<User>(Messages.UserNotFound);
        //    }

        //    if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
        //    {
        //        return new ErrorDataResult<User>(Messages.PasswordError);
        //    }

        //    return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
        //}

        //public IResult UserExists(string email)
        //{
        //    var result = _userService.GetByMail(email);
        //    if (result != null)
        //    {
        //        return new SuccessResult();
        //    }
        //    return new ErrorResult(Messages.UserAlreadyExists);
        //}

        //public IDataResult<AccessToken> CreateAccessToken(User user)
        //{
        //    var claims = _userService.GetClaims(user);
        //    var accessToken = _tokenHelper.CreateToken(user, claims.Data);
        //    return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        //}


        public async Task<IDataResult<UserForLoginDto>> BuildUserDtoAsync(Guid userId)
        {
            var user = await _context.Users
                //.Include(x => x.UserRoles.Where(x => !x.IsDeleted && !x.Role.IsDeleted))
                    //.ThenInclude(y => y.Role)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsDeleted == false);
            if (user == null)
            {
                return new ErrorDataResult<UserForLoginDto>("Kullanıcı bulunamadı!");
            }

            var userRoles = user.UserRoles.Where(x => !x.IsDeleted).Select(ur => new UserRoleDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                RoleId = ur.RoleId,
                RoleName = ur.Role.Name
            }).ToArray();

            var newUserDto = new UserForLoginDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                //ExpirationMinutes = _jwtService.GetMinutes,
                UserRoles = userRoles,
                UserRolesCsv = string.Join(", ", userRoles.Select(ur => ur.RoleName)),
                CurrentRoleId = user.RoleId ?? userRoles.Select(x => x.RoleId).First(),
                CurrentRoleName = user.Role?.Name ?? userRoles.Select(x => x.RoleName).First()
            };
            //newUserDto.Token = _jwtService.CreateToken(newUserDto);


            return new ErrorDataResult<UserForLoginDto>("Veri alma işlemi başarısız!");
        }

        public async Task UpdateCurrentRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            user.RoleId = roleId;
            user.ModifiedAt = DateTime.Now;
            user.ModifiedBy = Guid.Parse(_currentUserService.GetUserId());

            await _context.SaveChangesAsync();
        }

    }
}
