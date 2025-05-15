using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models;
using EmployeeApp.Models.ViewModel;

namespace EmployeeApp.Controllers
{
    public class SalarySlipController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: SalarySlip
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PdfSalarySlip(string month)
        {
            if (Session["UserId"] != null)
            {
                int employeeId = Convert.ToInt32(Session["UserId"]);

                var dataList = (from employee in db.Employees
                                join salary in db.Salarys on employee.EmployeeId equals salary.EmployeeId
                                join slip in db.salarySlips on salary.SalaryId equals slip.SalaryId
                                join department in db.Departments on employee.DepartmentId equals department.DepartmentId
                                join designation in db.Designations on employee.DesignationId equals designation.DesignationId
                                where salary.EmployeeId == employeeId && salary.Month == month
                                select new
                                {
                                    employee.EmployeeId,
                                    employee.EmployeeName,
                                    employee.ContactNo,
                                    employee.Email,
                                    department.DepartmentName,
                                    designation.DesignationName,
                                    salary.Basic,
                                    salary.HRA,
                                    salary.Allowances,
                                    salary.Deductions,
                                    salary.Month,
                                    salary.Year,
                                    slip.NetSalary
                                }).FirstOrDefault(); // Using `FirstOrDefault()` to return a single record

                if (dataList != null)
                {
                    return Json(new { exists = true, data = dataList }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { exists = false }, JsonRequestBehavior.AllowGet);
        }

        //display dashboard data salarty slip
        [HttpGet]
        public ActionResult GetsalaryDash()
        {
            try
            {
                int employeeId = Convert.ToInt32(Session["UserId"]);
                var dataList = (from slip in db.salarySlips
                                join salary in db.Salarys
                                on slip.SalaryId equals salary.SalaryId
                                join employee in db.Employees
                               on salary.EmployeeId equals employee.EmployeeId
                                where salary.EmployeeId == employeeId
                                select new
                                {
                                    employeeId = employee.EmployeeId,
                                    employeeName = employee.EmployeeName,
                                    GeneratedDate = slip.GeneratedDate,
                                    netSalary = slip.NetSalary
                                }).ToList();

                return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.InnerException?.Message;
                return Json(new { success = false, message = "Save failed: " + inner });
            }

        }

    }
}