using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models.ViewModel
{
    public class VMDepartment
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage ="please enter department name")]
        public string DepartmentName { get; set; }

    }
}

//controller :-- if(model.IsValid)
