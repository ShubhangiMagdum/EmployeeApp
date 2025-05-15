using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    public class AttendanceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Attendance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyAttendance()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAll(int? id)
        {
            var dataList = (from attendance in db.attendances
                            join employee in db.Employees
                            on attendance.EmployeeId equals employee.EmployeeId
                            where attendance.EmployeeId == id
                            select new
                            {
                                employeeId = employee.EmployeeId,
                                employeeName = employee.EmployeeName,
                                attendanceId=attendance.AttendanceId,
                                date=attendance.Date,
                                Status=attendance.status
                            }).ToList();

            return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Attendance attendance)
        {
             var newattendance = new Attendance
             {
                  EmployeeId = attendance.EmployeeId,
                  Date = attendance.Date,
                 status = attendance.status
              };

            db.attendances.Add(newattendance);
            db.SaveChanges();

            TempData["success"] = "Attendance saved successfully!";
            return RedirectToAction("Index");
        }
    }
}