using System.ComponentModel.DataAnnotations;

namespace DatabaseConfigClassLibrary.DTO
{
    public class AddressDetails
    {
        [Key]
        public string AddressId { get; set; }
        public int number { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int zipcode { get; set; }
    }
}
