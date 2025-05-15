using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Designation
    {

        [Key]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        public string DesignationDescription { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }


    }
}