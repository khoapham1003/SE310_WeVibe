using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeVibe.Core.Contracts.User;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }

        public async Task<string> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new ApplicationUser
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (createUserDto.Roles != null && createUserDto.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(user, createUserDto.Roles);
                if (!roleResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            return user.Id;
        }

        public async Task<(bool Success, List<string> Errors)> UpdateUserAsync(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return (false, new List<string> { "User not found." });

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return (false, result.Errors.Select(e => e.Description).ToList());

            if (dto.Roles != null && dto.Roles.Any())
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, dto.Roles);
            }

            return (true, null);
        }

        public async Task<(bool Success, List<string> Errors)> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return (false, new List<string> { "User not found." });

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? (true, null) : (false, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool Success, List<string> Errors)> ChangeUserRolesAsync(string id, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return (false, new List<string> { "User not found." });

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result.Succeeded ? (true, null) : (false, result.Errors.Select(e => e.Description).ToList());
        }
    }
}
