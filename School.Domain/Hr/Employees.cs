using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace School.Domain.Hr
{
    public class Employees
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FristName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateOnly JoinigDate { get; set; }

        public string designation_name { get; set; } = string.Empty;
        public string departments { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
