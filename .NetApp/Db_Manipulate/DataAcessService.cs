using Db_Manipulate.Models;
namespace Db_Manipulate
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
            return _dbContext.UserDatas.FirstOrDefault(u => u.email == email);
        }

        public void ImportUserData(IEnumerable<UserData> userDataList)
        {
            foreach (var userData in userDataList)
            {
                var newUser = new UserData
                {
                    _id = userData._id,
                    index = userData.index,
                    age = userData.age,
                    eyeColor = userData.eyeColor,
                    name = userData.name,
                    gender = userData.gender,
                    company = userData.company,
                    email = userData.email,
                    phone = userData.phone,
                    about = userData.about,
                    registered = userData.registered,
                    latitude = userData.latitude,
                    longitude = userData.longitude,
                    tags = userData.tags,
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

        public void ImportUserAddress(IEnumerable<AddressData> userAddressList)
        {
            foreach (var address in userAddressList)
            {
                var addressData = new AddressData
                {
                    AddressId = address.AddressId,
                    number = address.number,
                    street = address.street,
                    city = address.city,
                    state = address.state,
                    zipcode = address.zipcode
                };
                _dbContext.UserAddresses.Add(addressData);
                _dbContext.SaveChanges();
            }
        }
    }
}
