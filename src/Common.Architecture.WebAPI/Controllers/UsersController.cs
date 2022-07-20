using Common.Architecture.Core.Entities.ViewModels;
using Common.Architecture.Core.Utilities.Results;
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
        public async Task<IActionResult> SaveUser([FromForm] UserDto dto)
        {
            if (dto.Id == Guid.Empty) // NEW USER
            {
                var addResult = await _userService.AddAsync(dto, dto.Password, "");
                if (addResult.Success)
                {
                    return Ok(addResult);

                }
                return BadRequest(addResult);
            }

            var updateResult = await _userService.UpdateAsync(dto, dto.Password);

            if (updateResult.Success)
            {
                return Ok(updateResult);
            }

            return BadRequest();
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadDataAsync()
        {
            var vm = new DataTableViewModel(Request);

            return Ok(await _userService.LoadDataTableAsync(vm));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _userService.GetAllAsync();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var userDto = await _userService.GetByIdAsync(id);
            if (userDto == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
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

            //await _userService.UpdateCurrentRoleAsync(userId, roleId);

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
            return Ok();
        }
    }
}
