using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EmployeeApp.Data;
using EmployeeApp.Models;
using EmployeeApp.Models.ViewModel;

namespace EmployeeApp.Controllers
{
    public class SalaryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }




        [HttpGet]
        public ActionResult AddSalary(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var editEmployee = db.Employees.FirstOrDefault(e => e.EmployeeId == id);

            if (editEmployee == null)
            {
                return HttpNotFound();
            }

            // Map Employee to VMSalary
            var editSalary = new VMSalary
             {
                    EmployeeId = editEmployee.EmployeeId,
                    EmployeeName=editEmployee.EmployeeName
                    

            };

            //assign value viewbag 
            ViewBag.EmployeeId = editEmployee.EmployeeId;
           ViewBag.EmployeeName = editEmployee.EmployeeName;

            return View(editSalary);
            
        }

        //[HttpGet]
        //public ActionResult GetAll(int empId)
        //{
        //    var dataList = (from salary in db.Salarys
        //                    join employee in db.Employees
        //                    on salary.EmployeeId equals employee.EmployeeId
        //                    join slip in db.salarySlips
        //                    on salary.EmployeeId equals slip.EmployeeId
        //                    where salary.EmployeeId == empId
        //                    select new
        //                    {
        //                        employeeId = employee.EmployeeId,
        //                        employeeName = employee.EmployeeName,
        //                        salaryId = salary.SalaryId,
        //                        basic=salary.Basic,
        //                        hra=salary.HRA,
        //                        allowances=salary.Allowances,
        //                        deductions=salary.Deductions,
        //                        month=salary.Month,
        //                        year=salary.Year,
        //                        netSalary=slip.NetSalary
        //                    }).ToList();

        //    return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public ActionResult GetAll(int empId)
        {
            var dataList = (from employee in db.Employees
                            join salary in db.Salarys
                            on employee.EmployeeId equals salary.EmployeeId
                            join slip in db.salarySlips
                            on salary.SalaryId equals slip.SalaryId 
                            where salary.EmployeeId == empId
                            select new
                            {
                                employeeId = employee.EmployeeId,
                                employeeName = employee.EmployeeName,
                                salaryId = salary.SalaryId,
                                basic = salary.Basic,
                                hra = salary.HRA,
                                allowances = salary.Allowances,
                                deductions = salary.Deductions,
                                month = salary.Month,
                                year = salary.Year,
                                netSalary = slip.NetSalary
                            }).ToList();

            return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create(VMSalary sal)
        {
            if (ModelState.IsValid)
            {
                var existingRecord = db.Salarys.FirstOrDefault(c =>
                    c.EmployeeId == sal.EmployeeId &&
                    c.Month == sal.Month &&
                    c.Year == sal.Year);

                if (existingRecord != null)
                {
                    return Json(new { success = false, message = "Duplicate salary record exists!" });
                }

                var newSal = new Salary
                {
                    EmployeeId = sal.EmployeeId,
                    Basic = sal.Basic,
                    HRA = sal.HRA,
                    Allowances = sal.Allowances,
                    Deductions = sal.Deductions,
                    Month = sal.Month,
                    Year = sal.Year,
                    CreatedAt = DateTime.Now
                };
               db.Salarys.Add(newSal);
                db.SaveChanges();

                var newSlip = new SalarySlip
                {
                    SalaryId = newSal.SalaryId,
                    SalaryMonth = sal.Month,
                    NetSalary = sal.NetSalary,
                    GeneratedDate = DateTime.Now
                };
                db.salarySlips.Add(newSlip);

                 db.SaveChanges();
                TempData["Success"] = "Salary created successfully!";
                return new HttpStatusCodeResult(200);
            }

            return Json(new { success = false, message = "Invalid input data." });
            
        }


       
        [HttpPost]
        public ActionResult EditSalaryByEmployee(VMSalary sal)
        {
                var salary = db.Salarys.FirstOrDefault(s => s.SalaryId == sal.SalaryId);
                if (salary != null)
                {
                    // Update Salary
                    salary.Basic = sal.Basic;
                    salary.HRA = sal.HRA;
                    salary.Allowances = sal.Allowances;
                    salary.Deductions = sal.Deductions;
                    salary.Month = sal.Month;
                    salary.Year = sal.Year;
                    salary.CreatedAt = DateTime.Now;

                    db.Entry(salary).State = EntityState.Modified;

                    // Calculate NetSalary
                    decimal netSalary = (sal.Basic + sal.HRA + sal.Allowances) - sal.Deductions;

                    // Update SalarySlip
                    var slip = db.salarySlips.FirstOrDefault(s => s.SalaryId == sal.SalaryId && s.SalaryMonth == sal.Month);
                    if (slip != null)
                    {
                        slip.NetSalary = netSalary;
                        slip.GeneratedDate = DateTime.Now;
                        db.Entry(slip).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                TempData["Success"] = "Salary updated successfully!";
                return new HttpStatusCodeResult(200);
                // return Json(new { success = true, message = "Salary updated successfully!" });
            }

                return Json(new { success = false, message = "Salary record not found!" });
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var exitingSalary = db.Salarys.Where(c => c.SalaryId == id).FirstOrDefault();
            if (exitingSalary != null)
            {
                db.Salarys.Remove(exitingSalary);
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

        //display dashboard salaryslip count
        [HttpGet]
        public JsonResult GetSalarySlipCount()
        {
            int totalsalarySlip = db.salarySlips.Count();
            return Json(totalsalarySlip, JsonRequestBehavior.AllowGet);

        }



        //bar chart display in admin dashboard
        //Group all employees together based on their Department and Designation names.
        //it convert to array then display data in key-value pair 
        [HttpGet]
        public ActionResult SalaryBarChart()
        {
            try
            {
                var chartData = (from emp in db.Employees
                                 where emp.RoleId != 1
                                 join dept in db.Departments on emp.DepartmentId equals dept.DepartmentId into deptJoin
                                 from dept in deptJoin.DefaultIfEmpty()
                                 join desig in db.Designations on emp.DesignationId equals desig.DesignationId into desigJoin
                                 from desig in desigJoin.DefaultIfEmpty()
                                 join sal in db.Salarys on emp.EmployeeId equals sal.EmployeeId into salJoin
                                 from sal in salJoin.DefaultIfEmpty()
                                 group new { emp, dept, desig, sal } by emp.EmployeeId into g
                                 select new
                                 {
                                     EmployeeName = g.FirstOrDefault().emp.EmployeeName,
                                     Department = g.FirstOrDefault().dept != null ? g.FirstOrDefault().dept.DepartmentName : "N/A",
                                     Designation = g.FirstOrDefault().desig != null ? g.FirstOrDefault().desig.DesignationName : "N/A",
                                     Basic = g.Sum(x => x.sal != null ? x.sal.Basic : 0),
                                     HRA = g.Sum(x => x.sal != null ? x.sal.HRA : 0),
                                     Allowances = g.Sum(x => x.sal != null ? x.sal.Allowances : 0),
                                     Deductions = g.Sum(x => x.sal != null ? x.sal.Deductions : 0),
                                     NetSalary = g.Sum(x => x.sal != null ? (x.sal.Basic + x.sal.HRA + x.sal.Allowances - x.sal.Deductions) : 0)
                                 }).ToList();

                return Json(chartData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = "Error: " + inner });
            }
        }

    }
}