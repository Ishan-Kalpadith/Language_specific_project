using DatabaseConfigClassLibrary.DatabaseConfig;
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
                            u.Id.Contains(searchText)
                            || u.Name.Contains(searchText)
                            || u.Company.Contains(searchText)
                            || u.Email.Contains(searchText)
                            || u.Phone.Contains(searchText)
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
