using Db_Manipulate;
using Db_Manipulate.DTO;
using Microsoft.EntityFrameworkCore;

namespace Language_Specific_Project.Services
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
                    .Include(u => u.address)
                    .FirstOrDefaultAsync(u => u._id == _id);

                if (user == null)
                {
                    return (false, "User not found");
                }

                if (!string.IsNullOrEmpty(userUpdate.name))
                {
                    user.name = userUpdate.name;
                }

                if (!string.IsNullOrEmpty(userUpdate.email))
                {
                    user.email = userUpdate.email;
                }

                if (!string.IsNullOrEmpty(userUpdate.phone))
                {
                    user.phone = userUpdate.phone;
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
