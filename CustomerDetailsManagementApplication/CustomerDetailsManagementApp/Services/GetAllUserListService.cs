using System;
using System.Collections.Generic;
using DatabaseConfigClassLibrary.DatabaseConfig;
using DatabaseConfigClassLibrary.Repositories;

namespace CustomerDetailsManagementApp.Services
{
    public class GetAllCustomerListService
    {
        private readonly IUserRepository _userRepository;

        public GetAllCustomerListService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<object> GetAllCustomersAndAddresses()
        {
            try
            {
                return _userRepository.GetAllCustomersAndAddresses();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
