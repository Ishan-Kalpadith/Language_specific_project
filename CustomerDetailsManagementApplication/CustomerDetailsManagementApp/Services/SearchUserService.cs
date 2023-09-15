using DatabaseConfigClassLibrary;
using DatabaseConfigClassLibrary.Models;

namespace CustomerDetailsManagementApp.Services
{
    public class SearchUserService
    {
        private readonly ApplicationDbContext _context;

        public SearchUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserData> SearchUsers(string searchText)
        {
            try
            {
                var matchedUsers = _context.UserDatas
                    .Where(
                        u =>
                            u._id.Contains(searchText)
                            || u.name.Contains(searchText)
                            || u.company.Contains(searchText)
                            || u.email.Contains(searchText)
                            || u.phone.Contains(searchText)
                    )
                    .ToList();

                return matchedUsers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
