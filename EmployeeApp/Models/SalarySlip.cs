using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class SalarySlip
    {

        [Key]  
        public int SlipId { get; set; }
        public int SalaryId { get; set; }
        public string SalaryMonth { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}

//GeneratedDate :  current date not editable i.e. not change 
//salary Month ; get to salary table & show to input field not edittable i.e. not change 
//Net Salary : NetSalary = Basic + HRA + Allowances - Deductions
//operation in admin login  ;; add & deleted 
