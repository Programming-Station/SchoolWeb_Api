using System;
using System.Collections.Generic;
using System.Text;

namespace School_DTOs.School
{
    public class SchoolRegistrationDto
    {
        public int Id { get; set; }

        public string SchoolName { get; set; }

        public string SchoolCode { get; set; }

        public int EstablishedYear { get; set; }

        public string Address { get; set; }

        public string Pincode { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string Logo { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPersonName { get; set; }
    }
}
