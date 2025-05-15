using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models;
using EmployeeApp.Models.ViewModel;
using WebGrease.Css.Extensions;

namespace EmployeeApp.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Review
        public ActionResult Index()
        {
            var ratingsList = (from r in db.ratings
                               join e in db.Employees 
                               on r.EmployeeId equals e.EmployeeId
                               join department in db.Departments
                               on r.DepartmentId equals department.DepartmentId
                               join designation in db.Designations
                               on r.DesignationId equals designation.DesignationId
                               select new VMRating
                               {
                                   EmployeeId = r.EmployeeId,
                                   EmployeeName = e.EmployeeName, 
                                   Review = r.Review,
                                   Ratings = r.Ratings,
                                   DepartmentName=department.DepartmentName,
                                   DesignationName=designation.DesignationName
                               }).ToList();

            return View(ratingsList); 
        }
        public ActionResult AddReview()
        {
            if (!int.TryParse(Session["UserId"]?.ToString(), out int empId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid User ID");
            }

            var alreadyExist = db.ratings.FirstOrDefault(r => r.EmployeeId == empId);
            if (alreadyExist != null)
            {
                TempData["ErrorMessage"] = "You have already submitted a review.";
                ViewBag.HideForm = true; // Flag to hide form
                return View(); // Still render the view
            }

            var dataList = (from employee in db.Employees
                            join department in db.Departments on employee.DepartmentId equals department.DepartmentId
                            join designation in db.Designations on employee.DesignationId equals designation.DesignationId
                            where employee.EmployeeId == empId
                            select new VMRating
                            {
                                EmployeeId = employee.EmployeeId,
                                EmployeeName = employee.EmployeeName,
                                DepartmentId = department.DepartmentId,
                                DepartmentName = department.DepartmentName,
                                DesignationId = designation.DesignationId,
                                DesignationName = designation.DesignationName
                            }).FirstOrDefault();

            if (dataList == null)
            {
                return HttpNotFound();
            }

            ViewBag.EmployeeId = dataList.EmployeeId;
            ViewBag.EmployeeName = dataList.EmployeeName;
            ViewBag.DepartmentId = dataList.DepartmentId;
            ViewBag.DepartmentName = dataList.DepartmentName;
            ViewBag.DesigantionId = dataList.DesignationId;
            ViewBag.Designation = dataList.EmployeeName;

            ViewBag.HideForm = false; // Allow form to show
            return View(dataList);
        }

        //Add review page 
        //public ActionResult AddReview()
        //{
        //    int empId = Convert.ToInt32(Session["UserId"]);
        //    var aleadyExist = db.ratings.Where(r => r.EmployeeId == empId).FirstOrDefault();
        //    if(aleadyExist != null)
        //    {


        //        TempData["ErrorMessage"] = "You have already submitted a review.";
        //        return new HttpStatusCodeResult(500);
        //        //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Already Added the review.");
        //    }


        //    if (!int.TryParse(Session["UserId"]?.ToString(), out empId))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid User ID");
        //    }

        //    var dataList = (from employee in db.Employees
        //                    join department in db.Departments
        //                    on employee.DepartmentId equals department.DepartmentId
        //                    join designation in db.Designations
        //                    on employee.DesignationId equals designation.DesignationId
        //                    where employee.EmployeeId == empId
        //                    select new VMRating
        //                    {
        //                        EmployeeId = employee.EmployeeId,
        //                        EmployeeName = employee.EmployeeName,
        //                        DepartmentId = department.DepartmentId,
        //                        DepartmentName = department.DepartmentName,
        //                        DesignationId = designation.DesignationId,
        //                        DesignationName = designation.DesignationName
        //                    }).FirstOrDefault();


        //    if (dataList == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    //assign value viewbag 
        //    ViewBag.EmployeeId = dataList.EmployeeId;
        //    ViewBag.EmployeeName = dataList.EmployeeName;
        //    ViewBag.DepartmentId = dataList.DepartmentId;
        //    ViewBag.DepartmentName = dataList.DepartmentName;
        //    ViewBag.DesigantionId = dataList.DesignationId;
        //    ViewBag.Designation = dataList.EmployeeName;

        //    return View(dataList);
        //}


        [HttpPost]
        public  ActionResult Create(VMRating vMRating)
        {
            var newrating = new Rating
            {
                EmployeeId = vMRating.EmployeeId,
                DepartmentId = vMRating.DepartmentId,
                DesignationId = vMRating.DesignationId,
                Review = vMRating.Review,
                Ratings=vMRating.Ratings,
                CreatedAt=DateTime.Now,
                Createdby="1"
            };

            db.ratings.Add(newrating);
            db.SaveChanges();

            TempData["Success"] = "Rating saved successfully!";
            //return new HttpStatusCodeResult(200);
            return RedirectToAction("Dashboard", "Employee");
        }
    }
}