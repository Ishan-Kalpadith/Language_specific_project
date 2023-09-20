using DatabaseConfigClassLibrary.DTO;
using DatabaseConfigClassLibrary.Models;

namespace DatabaseConfigClassLibrary.DatabaseConfig
{
    public class DataAccessService
    {
        private readonly ApplicationDbContext _dbContext;

        public DataAccessService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public UserData GetUserByEmail(string email)
        {
            return _dbContext.UserDatas.FirstOrDefault(u => u.Email == email);
        }

        public void ImportUserData(IEnumerable<UserDTO> userDataList)
        {
            foreach (var userData in userDataList)
            {
                var newUser = new UserData
                {
                    Id = userData._id,
                    Index = userData.index,
                    Age = userData.age,
                    EyeColor = userData.eyeColor,
                    Name = userData.name,
                    Gender = userData.gender,
                    Company = userData.company,
                    Email = userData.email,
                    Phone = userData.phone,
                    About = userData.about,
                    Registered = userData.registered,
                    Latitude = userData.latitude,
                    Longitude = userData.longitude,
                    Tags = userData.tags,
                    AddressId = userData.AddressId
                };
                var trackedUser = _dbContext.UserDatas.Find(userData._id);

                if (trackedUser == null)
                {
                    _dbContext.UserDatas.Add(newUser);
                    _dbContext.SaveChanges();
                }
            }
        }

        public void ImportUserAddress(IEnumerable<AddressDetails> userAddressList)
        {
            foreach (var address in userAddressList)
            {
                var addressData = new AddressData
                {
                    AddressId = address.AddressId,
                    Number = address.number,
                    Street = address.street,
                    City = address.city,
                    State = address.state,
                    Zipcode = address.zipcode
                };
                _dbContext.UserAddresses.Add(addressData);
                _dbContext.SaveChanges();
            }
        }
    }
}
