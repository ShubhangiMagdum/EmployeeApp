using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        public int EmployeeId { get; set; }

        public int DepartmentId {  get; set; }

        public int DesignationId { get; set; }

        public int Ratings { get; set; }

        public string Review { get; set; }

        public string Createdby { get; set; }   

        public DateTime CreatedAt { get; set; }
    }
}