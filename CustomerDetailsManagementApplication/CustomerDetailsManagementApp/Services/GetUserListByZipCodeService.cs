using DatabaseConfigClassLibrary;

namespace CustomerDetailsManagementApp.Services
{
    public class GetCustomerListByZipCodeService
    {
        private readonly ApplicationDbContext _context;

        public GetCustomerListByZipCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<object> GetCustomersByZipCode()
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

                var groupedCustomers = customersAndAddresses
                    .GroupBy(data => data.addressData.zipcode)
                    .Select(
                        group =>
                            new
                            {
                                ZipCode = group.Key,
                                Customers = group.Select(data => data.userData).ToList()
                            }
                    )
                    .ToList();

                return groupedCustomers.Select(item => new 
                { 
                    item.ZipCode, item.Customers
                }).ToList<object>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
