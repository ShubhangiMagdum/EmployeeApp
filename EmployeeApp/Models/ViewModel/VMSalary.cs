using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models.ViewModel
{
    public class VMSalary
    {
        public int SalaryId { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Name is required.")]
        [StringLength(100, ErrorMessage = "Employee Name cannot exceed 100 characters.")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Basic Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Basic Salary must be a positive number.")]
        public decimal Basic { get; set; }
        [Required(ErrorMessage = "HRA is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "HRA must be a positive number.")]
        public decimal HRA { get; set; }

        [Required(ErrorMessage = "Allowances are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Allowances must be a positive number.")]
        public decimal Allowances { get; set; }
        [Required(ErrorMessage = "Deductions are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Deductions must be a positive number.")]
        public decimal Deductions { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        //salaryslip table
        public decimal NetSalary { get; set; }

    }
}