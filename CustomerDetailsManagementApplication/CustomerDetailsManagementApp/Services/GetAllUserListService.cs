using DatabaseConfigClassLibrary;

namespace CustomerDetailsManagementApp.Services
{
    public class GetAllCustomerListService
    {
        private readonly ApplicationDbContext _context;

        public GetAllCustomerListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<object> GetAllCustomersAndAddresses()
        {
            try
            {
                var customersAndAddresses = _context.UserDatas
                    .Join(
                        _context.UserAddresses,
                        userData => userData.AddressId,
                        addressData => addressData.AddressId,
                        (userData, addressData) => new { userData, addressData }
                    )
                    .ToList();

                return customersAndAddresses.Select(data => new
                {
                    data.userData,
                    data.addressData
                }).ToList<object>();
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
    }
}


/*data.userData._id,
                   data.userData.index,
                   data.userData.age,
                   data.userData.eyeColor,
                    data.userData.name,
                    data.userData.gender,
                    data.userData.company,
                    data.userData.email,
                    data.userData.phone,
                    data.userData.about,
                    data.userData.registered,
                    data.userData.latitude,
                    data.userData.longitude,
                    data.userData.tags,
                    data.userData._id,
                    data.userData.name,
                    data.userData._id,
                    data.userData.name,
                    data.addressData.zipcode*/