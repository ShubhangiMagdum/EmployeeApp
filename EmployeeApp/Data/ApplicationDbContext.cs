using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EmployeeApp.Models;

namespace EmployeeApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<Salary> Salarys { get; set; }

        public DbSet<EmployeeLeave> EmployeesLeave { get; set; }

        public DbSet<SalarySlip> salarySlips { get; set; }

        public DbSet<Attendance> attendances { get; set; }

        public  DbSet<Rating> ratings { get; set; }

    }

}