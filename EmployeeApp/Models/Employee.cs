using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

         public string EmployeeName { get; set; }
        
         public string Email { get; set; }

         public  string ContactNo { get; set; }
       public string PasswordHash { get; set; }
       public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string profileImg { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }

        public int RoleId { get; set; }



    }
}