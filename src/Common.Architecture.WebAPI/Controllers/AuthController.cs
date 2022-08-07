using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Entities.ViewModels;
using Common.Architecture.Infrastructure.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Architecture.WebAPI.Controllers
{
    [Route("api/[controller")]
    [ApiController]
    [Authorize]
    public class AuthController: Controller
    {
        private readonly IAuthService _authService; 
        //private readonly IJwtService _jwtService;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService,
            //IJwtService jwtService,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
        IUserService userService)
        {
            //_jwtService = jwtService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
        }


        [HttpGet]
        public async Task<ActionResult<UserForLoginDto>> GetCurrentUser()
        {
            var userId = Guid.Parse(HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
            if (userId == Guid.Empty)
            {
                return BadRequest("LoginError400");
            }

            //var user = await _userService.GetByIdAsync(userId);
            //if (user == null)
            //{
            //    //User not found!
            //    return BadRequest("LoginError400");
            //}
            var result = await _userService.BuildUserDtoAsync(userId);

            return Ok(result.Value);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromForm] RegisterDto dto)
        {
            if (CheckEmailExists(dto.Email).Result.Value)
            {
                return BadRequest("Email is in use!");
            }

            var user = _mapper.Map<User>(dto);
            user.Email = user.Email.ToLower();
            user.UserName = user.Email.ToLower();
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Üye");
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            // SEND Confirmation Email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"email", user.Email },
                {"token", token }
            };
            //var clientURI = _config["FrontendUrl"] + "pages/auth/login";
            var clientURI = "http://localhost:33070/Account/ConfirmEmail";
            var callback = QueryHelpers.AddQueryString(clientURI, param);
            var fullName = dto.Name + " " + dto.Surname;
            var toEmail = user.Email; //"istech@mailinator.com";
            var subject = "Eposta Doğrulama";
            var emailBodyHtml = $@"
                Merhaba {fullName},
                <br/>
                Lütfen Eposta adresini doğrulamak için <a href='{callback}'>tıkla</a>.";

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Title = user.Title,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRolesCsv = "Üye"
            };
        }


        [AllowAnonymous]
        [HttpGet("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string email, string token)
        {
            if (email == null || token == null)
            {
                return BadRequest("Invalid email or token!");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Email not found!");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

            return Ok(new { emailConfirmed = true });
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromForm] LoginDto dto)
        {
            if (dto.Email == null || dto.Password == null)
                return NotFound("EPosta / Şifre hatalı giriş yapıldı!");

            UserDto userDto = new UserDto();
            long personelTanimId = 0;
            long personelSicilNo = 0;
            PersonelTanimDto personelTanim = null;
            //var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            //    x.Email.ToLower() == dto.Email.ToLower() && !x.IsDeleted);
            if (!dto.Email.Contains("@"))
            {
                //  var sonuc = Functions.mysfADAuthenticate("DPTDOMAIN", "10.10.1.25", "aydin.mehmet", "Plkmn1234+");
                var sonuc = Functions.mysfADAuthenticate(dto.Email, dto.Password);
                if (!string.IsNullOrEmpty(sonuc))
                {
                    return Unauthorized("EPosta / Şifre hatalı giriş yapıldı!");
                }

                userDto = await _userService.GetByLdapAsync(dto.Email);
                if (userDto == null)
                {
                    userDto = new UserDto();
                    userDto.Email = dto.Email + "@" + HRM.Application.ApplicationData.ApplicationSettings.Where(x => x.Ad == "DomainEmail").FirstOrDefault().Deger;
                    userDto.LdapKod = dto.Email;
                    userDto.Password = dto.Password;
                    userDto.Name = dto.Email;
                    userDto.Surname = dto.Email;
                    var personelKontrol = await _personelTanimService.GetByDomainAdAsync(userDto.LdapKod);
                    if (personelKontrol.Status != Ardalis.Result.ResultStatus.Ok)
                        return BadRequest("LoginError400");
                    personelTanim = personelKontrol.Value;
                    personelTanimId = personelTanim.Id;
                    personelSicilNo = personelTanim.KurumSicilNo;
                    userDto.Name = personelTanim.Ad;
                    userDto.Surname = personelTanim.Soyad;
                    userDto.Title = personelTanim.GorevTanimIdAd;
                    userDto = await _userService.AddAsync(userDto, dto.Password, "Kullanici");
                    userDto = await _userService.GetByEmailAsync(userDto.Email);
                }
            }

            else
            {
                userDto = await _userService.GetByEmailAsync(dto.Email);


                if (userDto.Email == null)
                {
                    //User not found!
                    return NotFound("Hatalı kullanıcı bilgisi!");
                }

                var resultUserLogin = await _signInManager.CheckPasswordSignInAsync(_mapper.Map<User>(userDto), dto.Password, false);
                if (!resultUserLogin.Succeeded)
                {
                    return Unauthorized("EPosta / Şifre hatalı giriş yapıldı!");
                }

                // rolu olmayan login olamaz!!!
                if (!userDto.UserRoles.Any())
                {
                    //user does not have any valid roles
                    return NotFound("Kullanıcıya ait geçerli bir rol bulunamadı!");
                }
            }

            if (personelTanim == null)
            {
                personelTanim = (await _personelTanimService.GetByDomainAdAsync(userDto.LdapKod)).Value;
                if (personelTanim != null)
                {
                    personelTanimId = personelTanim.Id;
                    personelSicilNo = personelTanim.KurumSicilNo;
                }
            }
            await _userService.LogUserAction(userDto.Id, "Login");
            var result = await _userService.BuildUserDtoAsync(userDto.Id);
            result.Value.PersonelTanimId = personelTanimId;
            result.Value.PersonelSicilNo = personelSicilNo;
            result.Value.SorumluBirimList = "";
            if (personelTanimId > 0)
            {
                //var sorumluKurumList = string.Join(',', ApplicationData.BirimList.Where(x => x.PersonelTanimId == personelTanimId).Select(x => x.Id).ToArray());
                var sorumluKurumList = ApplicationData.BirimList.Where(x => x.PersonelTanimId == personelTanimId).Select(x => x.Id).ToArray();
                var bagliBirimList = "";
                if (sorumluKurumList.Length > 0)
                {
                    foreach (var item in sorumluKurumList)
                    {
                        bagliBirimList += string.Join(',', ApplicationData.BirimList.Where(x => x.UstKurumBilgi.Contains(item.ToString())).Select(x => x.Id).ToArray()) + ",";
                    }
                    result.Value.SorumluBirimList = bagliBirimList.Remove(bagliBirimList.Length - 1, 1);
                }

            }
            return Ok(result.Value);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("loginAsUser")]
        public async Task<ActionResult<UserDto>> LoginAsUser([FromForm] string email)
        {
            var adminId = Guid.Parse(HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
            var userDto = await _userService.GetByEmailAsync(email);
            if (userDto == null)
            {
                //User not found!
                return BadRequest("LoginError400");
            }

            // rolu olmayan login olamaz!!!
            if (!userDto.UserRoles.Any())
            {
                //user does not have any valid roles
                return Unauthorized("LoginError401");
            }

            // bypass password control but add to AuditLog
            await _userService.LogUserAction(adminId, "Login", userDto.Id.ToString());
            var result = await _userService.BuildUserDtoAsync(userDto.Id);

            return Ok(result.Value);
        }



        [AllowAnonymous]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok();
            }

            //var token = HttpUtility.UrlEncode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

            return Ok(new { resetPassword = true });
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto model)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
            if (userId == Guid.Empty)
            {
                return BadRequest("LoginError400");
            }
            var userDto = await _userService.GetByIdAsync(userId);
            if (userDto == null)
            {
                //User not found!
                return BadRequest("LoginError400");
            }

            //var result = await _userManager.ChangePasswordAsync(userDto, model.CurrentPassword, model.NewPassword);
            //if (!result.Succeeded)
            //{
            //    return BadRequest(result.Errors.Select(error => error.Description));
            //}

            return Ok(new { passwordChanged = true });
        }


        [HttpPost("load")]
        public async Task<IActionResult> LoadData()
        {
            var vm = new DataTableViewModel(Request);

            return await _userService.LoadDataTable(vm);
        }


        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }






        //[HttpPost("login")]
        //public ActionResult Login(UserForLoginDto userForLoginDto)
        //{
        //    var userToLogin = _authService.Login(userForLoginDto);
        //    if (!userToLogin.Success)
        //    {
        //        return BadRequest(userToLogin.Message);
        //    }

        //    var result = _authService.CreateAccessToken(userToLogin.Data);
        //    if (result.Success)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return BadRequest(result.Message);
        //}

        //[HttpPost("register")]
        //public ActionResult Register(UserForRegisterDto userForRegisterDto)
        //{
        //    var userExists = _authService.UserExists(userForRegisterDto.Email);
        //    if (!userExists.Success)
        //    {
        //        return BadRequest(userExists.Message);
        //    }

        //    var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
        //    var result = _authService.CreateAccessToken(registerResult.Data);
        //    if (result.Success)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return BadRequest(result.Message);
        //}
    }
}
