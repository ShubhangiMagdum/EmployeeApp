using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}