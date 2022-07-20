using Common.Architecture.Core.Entities.ViewModels;
using Common.Architecture.Infrastructure.Abstract;
using Common.Architecture.Shared.TransferObjects.Idendity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommmonArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<UserDto> SaveUser([FromForm] UserDto dto)
        {
            if (dto.Id == Guid.Empty) // NEW USER
            {
                var addResult = await _userService.AddAsync(dto, dto.Password, "");
                if (addResult.Status != ResultStatus.Ok)
                {
                    return Result<UserDto>.Invalid(addResult.ValidationErrors);
                }

                return Result<UserDto>.Success(addResult.Value);
            }

            // EXISTING USER            
            var updateResult = await _userService.UpdateAsync(dto, dto.Password);
            switch (updateResult.Status)
            {
                case ResultStatus.Error:
                    return Result<UserDto>.Error();
                case ResultStatus.Forbidden:
                    return Result<UserDto>.Forbidden();
                case ResultStatus.Invalid:
                    return Result<UserDto>.Invalid(updateResult.ValidationErrors);
                case ResultStatus.NotFound:
                    return Result<UserDto>.NotFound();
                default:
                    break;
            }

            return Result<UserDto>.Success(updateResult.Value);
        }

        [HttpPost("load")]
        public async Task<ActionResult> LoadDataAsync()
        {
            var vm = new DataTableViewModel(Request);

            return await _userService.LoadDataTableAsync(vm);
        }

        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllAsync()
        {
            var result = await _carService.GetAllAsync();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
        {
            var userDto = await _userService.GetByIdAsync(id);
            if (userDto == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var userToDelete = await _userService.GetByIdAsync(id);
            if (userToDelete == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            await _userService.DeleteAsync(id);

            return Ok(new { userDeleted = true });
        }

        [HttpPut("{id}/role/{roleId}")]
        public async Task<ActionResult> SetCurrentRole(Guid roleId)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
            if (userId == Guid.Empty)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            await _userService.UpdateCurrentRoleAsync(userId, roleId);

            return Ok(new { currentRoleSet = true });
        }

        [HttpGet("TestZApiCall")]
        public async Task<ActionResult> LoadData2()
        {
            DataTableViewModel vm = new DataTableViewModel()
            {
                Draw = "",
                PageSize = 10,
                Skip = 0,
                SearchValue = "",
                SortColumnDirection = "",
                SortColumn = ""
            };

            //return await _userService.LoadDataTable(vm);
            return Ok;
        }
    }
}
