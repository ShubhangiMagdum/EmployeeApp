using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeApp.Models
{
    public class EmployeeLeave
    {
        [Key]
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public int TotalDays { get; set; }
        public string LeaveStatus { get; set; }
    
    }
}


//LeaveStatus dropdown option Approval, Cancel, pending 
//employee login when click leave edit, add, view
//if employee cancel leave auto. deleted record in table 