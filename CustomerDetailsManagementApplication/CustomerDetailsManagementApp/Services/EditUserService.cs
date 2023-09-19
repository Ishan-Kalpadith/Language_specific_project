using AutoMapper;
using DatabaseConfigClassLibrary.DatabaseConfig;
using DatabaseConfigClassLibrary.DTO;
using DatabaseConfigClassLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CustomerDetailsManagementApp.Services
{
    public class EditUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public EditUserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<(bool success, string message)> EditUserAsync(
            string _id,
            UserUpdateDTO userUpdate
        )
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(_id);

                if (user == null)
                {
                    return (false, "User not found");
                }

                if (!string.IsNullOrEmpty(userUpdate.Name))
                {
                    user.Name = userUpdate.Name;
                }

                if (!string.IsNullOrEmpty(userUpdate.Email))
                {
                    user.Email = userUpdate.Email;
                }

                if (!string.IsNullOrEmpty(userUpdate.Phone))
                {
                    user.Phone = userUpdate.Phone;
                }

                await _userRepository.UpdateUserAsync(user);

                return (true, "User updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}
