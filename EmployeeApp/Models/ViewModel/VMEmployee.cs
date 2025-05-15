using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models.ViewModel
{
    public class VMEmployee
    {

        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Name is required.")]
        [StringLength(100, ErrorMessage = "Employee Name can't be longer than 100 characters.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number must be 10 digits.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string PasswordHash { get; set; }


        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }


        [Required(ErrorMessage = "Designation is required.")]
        public int DesignationId { get; set; }


        public string profileImg { get; set; }

        public string DepartmentName { get; set; }

        public string DesignationName { get; set; }

        public string OriginalPassword { get; set; }

        public string NewPassword { get; set; }
    }
}