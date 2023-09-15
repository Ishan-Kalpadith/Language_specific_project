using DatabaseConfigClassLibrary.Models;
using DatabaseConfigClassLibrary.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DatabaseConfigClassLibrary
{
    public class DataImporter
    {
        private readonly DataAccessService _dataService;

        public DataImporter(DataAccessService dataService)
        {
            _dataService = dataService;
        }

        string filePath =
            "E:\\FIdenz Training Materials\\Language Specific Project\\CR\\Language_Specific_Project\\CustomerDetailsManagementApplication\\DatabaseConfigClassLibrary\\UserData.json";

        public void ImportDataFromJson()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<List<UserDTO>>(json);

                var addressDictionary = new Dictionary<string, AddressDetails>();

                foreach (var User in data)
                {
                    var existingUser = _dataService.GetUserByEmail(User.email);

                    if (existingUser == null)
                    {
                        var address = User.address;
                        var addressData = new AddressDetails
                        {
                            number = address.number,
                            street = address.street,
                            city = address.city,
                            state = address.state,
                            zipcode = address.zipcode,
                            AddressId = GenerateUniqueAddressId()
                        };
                        addressDictionary[addressData.AddressId] = addressData;
                        User.AddressId = addressData.AddressId;
                        User.address = addressData;
                    }
                    else
                    {
                        Console.WriteLine(
                            $"User with Email {User.email} already exists, skipping insertion."
                        );
                    }
                }
                _dataService.ImportUserAddress(addressDictionary.Values);
                _dataService.ImportUserData(data);
            }
            else
            {
                Console.WriteLine("The UserData.json file does not exist at the specified path");
            }
        }

        // generate a unique AddressId
        private string GenerateUniqueAddressId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
