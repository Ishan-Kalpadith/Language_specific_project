using DatabaseConfigClassLibrary;
using DatabaseConfigClassLibrary.DTO;
using Microsoft.EntityFrameworkCore;

namespace CustomerDetailsManagementApp.Services
{
    public class EditUserService
    {
        private readonly ApplicationDbContext _context;

        public EditUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message)> EditUserAsync(string _id, UserUpdateDTO userUpdate)
        {
            try
            {
                var user = await _context.UserDatas
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id == _id);

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

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return (true, "User updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}
