using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseConfigClassLibrary.DTO
{
    public class UserDTO
    {
        [Key]
        public string? _id { get; set; }
        public int? index { get; set; }
        public int? age { get; set; }
        public string? eyeColor { get; set; }
        public string? name { get; set; }
        public string? gender { get; set; }
        public string? company { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? about { get; set; }
        public string? registered { get; set; }
        public long? latitude { get; set; }
        public float? longitude { get; set; }
        public List<string>? tags { get; set; }
        public AddressDetails? address { get; set; }
        public string AddressId { get; set; }
    }
}
