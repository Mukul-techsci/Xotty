using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class enAddress
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? FullName { get; set; }

        public string? MobileNo { get; set; }

        public string? AddressLine { get; set; }

        public string? City { get; set; }

        public string? StateName { get; set; }

        public string? Pincode { get; set; }

        public string? AddressType { get; set; }

        public bool IsDefault { get; set; }

        public int Status { get; set; }

        public string? Message { get; set; }
    }
}
