using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models;
using EmployeeApp.Models.ViewModel;

namespace EmployeeApp.Controllers
{
    public class LeaveController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Leave
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddLeave()
        {
            return View(new EmployeeLeave());

        }

        [HttpPost]
        public ActionResult Create(EmployeeLeave leave)
        {

            var empId = Session["UserId"] as int?;
            if(empId !=null)
            {
                if(leave.LeaveId==0)
                {

                    var newleave = new EmployeeLeave
                    {
                        EmployeeId = leave.EmployeeId,
                        LeaveType = leave.LeaveType,
                        Reason = leave.Reason,
                        LeaveStartDate = leave.LeaveStartDate,
                        LeaveEndDate = leave.LeaveEndDate,
                        TotalDays = leave.TotalDays,
                        LeaveStatus = "Pending",
                    };

                    db.EmployeesLeave.Add(newleave);
                    db.SaveChanges();

                    TempData["Success"] = "Employee Leave saved successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var existingleave = db.EmployeesLeave.Find(leave.LeaveId);
                    if (existingleave != null)
                    {
                        existingleave.EmployeeId = leave.EmployeeId;
                        existingleave.LeaveType = leave.LeaveType;
                        existingleave.Reason = leave.Reason;
                        existingleave.LeaveStartDate = leave.LeaveStartDate;
                        existingleave.LeaveEndDate = leave.LeaveEndDate;
                        existingleave.TotalDays = leave.TotalDays;
                        db.Entry(existingleave).State = EntityState.Modified;
                    }

                }
                db.SaveChanges();
                TempData["Success"] = "Employee Leave Update successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }




        [HttpGet]
        public ActionResult GetAll()
        {

            if (Session["UserId"] != null && Session["UserRole"] != null)
            {
                int empId = Convert.ToInt32(Session["UserId"]);
                string role = Session["UserRole"].ToString();

                var dataList = db.EmployeesLeave
                     .Where(leave =>
                         (role == "Admin") ||
                         (leave.EmployeeId == empId &&
                          (leave.LeaveStatus == "Pending"))
                     )
                     .Select(leave => new
                     {
                         leaveId = leave.LeaveId,
                         employeeId = leave.EmployeeId,
                         leaveType = leave.LeaveType,
                         reason = leave.Reason,
                         leaveStartDate = leave.LeaveStartDate,
                         leaveEndDate = leave.LeaveEndDate,
                         totalDays = leave.TotalDays,
                         leaveStatus = leave.LeaveStatus,
                         role=role
                     })
                     .ToList();


                return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);

            }

            return RedirectToAction("Index", "Account");
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var leave = db.EmployeesLeave.Find(id);
            if (leave != null)
            {
                leave.LeaveStatus = "Cancel";
                db.Entry(leave).State = EntityState.Modified;
            }
        db.SaveChanges();
         return Json(new { success = true });

        }

        //Approved
        [HttpPost]
        public ActionResult Approved(int id)
        {
            var leave = db.EmployeesLeave.Find(id);
            if (leave != null)
            {
                leave.LeaveStatus = "Approved";
                db.Entry(leave).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new { success = true });

        }


        //display dashboard pending leave count
        [HttpGet]
        public JsonResult GetPendingCount()
        {
            int pendingLeaveCount = db.EmployeesLeave.Count(l => l.LeaveStatus == "Pending");
            return Json(pendingLeaveCount, JsonRequestBehavior.AllowGet);

        }

    }
}