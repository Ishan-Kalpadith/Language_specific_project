using System.ComponentModel.DataAnnotations;

namespace DatabaseConfigClassLibrary.Models
{
    public class UserData
    {
        [Key]
        public string Id { get; set; }
        public int Index { get; set; }
        public int Age { get; set; }
        public string? EyeColor { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? About { get; set; }
        public string? Registered { get; set; }
        public long? Latitude { get; set; }
        public float? Longitude { get; set; }
        public List<string>? Tags { get; set; }
        public AddressData? Address { get; set; }
        public string AddressId { get; set; }
    }

    public class AddressData
    {
        [Key]
        public string AddressId { get; set; }
        public int Number { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }
    }
}
