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


/*data.userData.Id,
                   data.userData.Index,
                   data.userData.Age,
                   data.userData.EyeColor,
                    data.userData.Name,
                    data.userData.Gender,
                    data.userData.Company,
                    data.userData.Email,
                    data.userData.Phone,
                    data.userData.About,
                    data.userData.Registered,
                    data.userData.Latitude,
                    data.userData.Longitude,
                    data.userData.Tags,
                    data.userData.Id,
                    data.userData.Name,
                    data.userData.Id,
                    data.userData.Name,
                    data.addressData.Zipcode*/