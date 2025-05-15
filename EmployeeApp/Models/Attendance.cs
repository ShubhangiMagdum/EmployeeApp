using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public string status { get; set; }
    }
}


//Status ;; 'Present', 'Absent', 'Half-day'
//Date :: current date 
//EmployeeId :: session throught get & show also display name 
//employee login in  after add attendence employee id wise  (operation :- add & edit) 