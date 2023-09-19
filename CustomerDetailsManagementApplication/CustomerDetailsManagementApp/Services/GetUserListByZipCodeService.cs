using System;
using System.Collections.Generic;
using DatabaseConfigClassLibrary.Repositories;

namespace CustomerDetailsManagementApp.Services
{
    public class GetCustomerListByZipCodeService
    {
        private readonly IUserRepository _userRepository;

        public GetCustomerListByZipCodeService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<object> GetCustomersByZipCode()
        {
            try
            {
                return _userRepository.GetCustomersByZipCode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
