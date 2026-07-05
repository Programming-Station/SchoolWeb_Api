using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace School_DTOs.Hr
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string? ApplicationUserId { get; set; }
        public string? EmployeePhoto { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime JoiningDate { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public int? SpecializationId { get; set; }
        public string? SpecializationName { get; set; }
        public decimal Experience { get; set; }
        public string? Qualification { get; set; }
    }

    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? PANNumber { get; set; }
        public string Email { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Status { get; set; } = "active";
        public DateTime JoiningDate { get; set; }
        public decimal Experience { get; set; } = 0;
        public int DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? SpecializationId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? EmploymentStatusId { get; set; }
        public int? SalaryGradeId { get; set; }
        public int? ShiftId { get; set; }
        public decimal WorkingHours { get; set; } = 8;
        public IFormFile? PhotoFile { get; set; }
    }

    public class UpdateEmployeeDto : CreateEmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = null!;
    }
}
