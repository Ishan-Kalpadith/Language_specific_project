using DatabaseConfigClassLibrary.Models;
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
            "C:\\Users\\Ishan Kalpadith\\source\\repos\\Language_project\\DatabaseConfigClassLibrary\\UserData.json";

        public void ImportDataFromJson()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<List<UserData>>(json);

                var addressDictionary = new Dictionary<string, AddressData>();

                foreach (var userData in data)
                {
                    var existingUser = _dataService.GetUserByEmail(userData.email);

                    if (existingUser == null)
                    {
                        var address = userData.address;
                        var addressData = new AddressData
                        {
                            number = address.number,
                            street = address.street,
                            city = address.city,
                            state = address.state,
                            zipcode = address.zipcode,
                            AddressId = GenerateUniqueAddressId()
                        };
                        addressDictionary[addressData.AddressId] = addressData;
                        userData.AddressId = addressData.AddressId;
                        userData.address = addressData;
                    }
                    else
                    {
                        Console.WriteLine(
                            $"User with email {userData.email} already exists, skipping insertion."
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
