using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models.ViewModel
{
    public class VMRating
    {
        public int RatingId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        public int Ratings { get; set; }

        public string Review { get; set; }
    }
}