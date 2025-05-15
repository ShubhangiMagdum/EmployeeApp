using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Salary
    {
        [Key]
       public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
       public decimal Basic { get; set; }
        public decimal HRA { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }




    }
}