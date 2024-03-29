﻿using AutoMapper;
using Common.Architecture.Core.Entities.Concrete;
using Common.Architecture.Core.Entities.ViewModels;
using Common.Architecture.Core.Utilities.Results;
using Common.Architecture.Infrastructure.Abstract;
using Common.Architecture.Persistance;
using Common.Architecture.Persistance.Abstract;
using Common.Architecture.Shared.TransferObjects.Idendity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Architecture.Infrastructure.Concrete
{
    public class UserService : IUserService
    {
        IUserDal _userDal;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        CommonDBContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(IUserDal userDal, UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userDal = userDal;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = new CommonDBContext();
        }

        public async Task<IResult> AddAsync(UserForLoginDto dto, string password, string rolAd)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                return new ErrorResult("Ekleme işlemi başarısız");
            }
            else
            {
                var user = _mapper.Map<User>(dto);

                await _userDal.AddAsync(user);

                return new SuccessResult("Kaydetme işlemi başarı ile tamamlandı!");
            }
        }
        public async Task<IResult> UpdateAsync(UserForLoginDto dto, string password)
        {
            if (dto != null)
            {
                var user = _mapper.Map<User>(dto);

                await _userDal.UpdateAsync(user);

                return new SuccessResult("Güncelleme işlemi başarı ile tamamlandı!");
            }

            return new ErrorResult("Ekleme işlemi başarısız");

        }

        public async Task<IResult> DeleteAsync(Guid userId)
        {
            await _userDal.DeleteAsync(x => x.Id == userId && x.IsActive && !x.IsDeleted);

            return new SuccessResult("Silme işlemi başarı ile tamamlandı!");
        }

        public async Task<IDataResult<List<UserForLoginDto>>> GetAllAsync()
        {

            var user = await _userDal.GetAllAsync();

            if (user != null)
            {
                var userList = _mapper.Map<List<UserForLoginDto>>(user);

                return new SuccessDataResult<List<UserForLoginDto>>(userList, "Liste alma başarı ile tamamlandı!");
            }

            return new ErrorDataResult<List<UserForLoginDto>>("Liste alma işlemi başarısız!");
        }
        public async Task<IDataResult<UserForLoginDto>> GetByIdAsync(Guid id)
        {

            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            //var userInfo = await _userDal.GetFirstOrDefaultAsync(x => x.Id == id && x.IsActive && !x.IsDeleted);

            if (userInfo != null)
            {
                var user = _mapper.Map<UserForLoginDto>(userInfo);

                return new SuccessDataResult<UserForLoginDto>(user, "Veri alma başarı ile tamamlandı!");
            }

            return new ErrorDataResult<UserForLoginDto>("Veri alma işlemi başarısız!");
        }

        public async Task<IDataResult<UserResponseDto>> GetByEmailAsync(string email)
        {

            var userInfo = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            //var userInfo = await _userDal.GetFirstOrDefaultAsync(x => x.Id == id && x.IsActive && !x.IsDeleted);

            if (userInfo != null)
            {
                var user = _mapper.Map<UserResponseDto>(userInfo);

                return new SuccessDataResult<UserResponseDto>(user, "Veri alma başarı ile tamamlandı!");
            }

            return new ErrorDataResult<UserResponseDto>("Veri alma işlemi başarısız!");
        }

        public async Task<IDataResult<JsonResult>> LoadDataTableAsync(DataTableViewModel vm, bool isActive = true, bool isDeleted = false)
        {
            //var queryAll = await _userDal.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.UserRoles.Role);

            int recordsTotal = await _userDal.CountAsync(x => x.IsActive && !x.IsDeleted);


            int recordsFiltered = recordsTotal;

            //var queryAll = _context.Users
            //        .Include(x => x.UserRoles.Where(x => !x.IsDeleted && !x.Role.IsDeleted))
            //            .ThenInclude(y => y.Role)
            //        .Where(x => !x.IsDeleted);

            ////_context.Users
            ////    .Include(x => x.UserRoles.Where(x => !x.IsDeleted && !x.Role.IsDeleted))
            ////        .ThenInclude(y => y.Role)
            ////    .Where(x => !x.IsDeleted);

            //var query = queryAll.Select(tempUser => new
            //{
            //    Id = tempUser.Id,
            //    Name = tempUser.Name,
            //    Surname = tempUser.Surname,
            //    Email = tempUser.Email,
            //    PhoneNumber = tempUser.PhoneNumber,
            //    Title = tempUser.Title,
            //    RoleNames = tempUser.UserRoles
            //        .Where(x => !x.IsDeleted && !x.Role.IsDeleted)
            //        .Select(x => x.Role.Name),
            //    Roles = string.Join(", ", tempUser.UserRoles
            //        .Where(x => !x.IsDeleted && !x.Role.IsDeleted)
            //        .Select(x => x.Role.Name))
            //});

            ////Sorting
            //if (!string.IsNullOrEmpty(vm.SortColumn) && !string.IsNullOrEmpty(vm.SortColumnDirection))
            //{
            //    query = query.OrderBy(vm.SortColumn + " " + vm.SortColumnDirection);
            //}

            ////Search
            //if (!string.IsNullOrEmpty(vm.SearchValue))
            //{
            //    query = query.Where(m =>
            //        m.Name.ToLower().Contains(vm.SearchValue.ToLower()) ||
            //        m.Surname.ToLower().Contains(vm.SearchValue.ToLower()) ||
            //        m.Email.ToLower().Contains(vm.SearchValue.ToLower()) ||
            //        m.RoleNames.Any(x => x.Contains(vm.SearchValue.ToLower())));

            //    recordsFiltered = await query.CountAsync();
            //}

            ////var data = userData.Skip(vm.Skip).Take(vm.PageSize);
            //var data = await query.Skip(vm.Skip).Take(vm.PageSize).ToListAsync();

            return new SuccessDataResult<JsonResult>(new JsonResult(new
            {
                draw = vm.Draw,
                recordsFiltered = recordsFiltered,
                recordsTotal = recordsTotal,
                //data = data
            }));
        }

        public Task<IDataResult<UserForLoginDto>> GetByIdAsync(Guid id, bool isDeleted = false)
        {
            throw new NotImplementedException();
        }
    }
}
