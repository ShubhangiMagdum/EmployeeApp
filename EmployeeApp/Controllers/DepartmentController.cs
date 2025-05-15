using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using EmployeeApp.Data;
using EmployeeApp.Models;
using EmployeeApp.Models.ViewModel;

namespace EmployeeApp.Controllers
{
    //[Authorize]
    public class DepartmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        //get all list 
        [HttpGet]
        public ActionResult GetAll()
        {
            var deptList = db.Departments.ToList();
            return Json(new { data = deptList }, JsonRequestBehavior.AllowGet);
        }

        //Dropdown list show in in _partial Employee
        [HttpGet]
        public JsonResult GetDepartment()
        {
            var departmentList = db.Departments.Select(c => new
            {
                c.DepartmentId,
                c.DepartmentName
            }).ToList();
            return Json(departmentList, JsonRequestBehavior.AllowGet);
        }



        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return PartialView("_AddOrEdit", new Department());
            else
            {
                var emp = db.Departments.Find(id);
                return PartialView("_AddOrEdit", emp);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(VMDepartment dep)
        {

            try
            {
                if (ModelState.IsValid)
                { 
                if (dep.DepartmentId == 0)
                {
                    var exitingdept = db.Departments.Where(c => c.DepartmentName == dep.DepartmentName).FirstOrDefault();
                        if (exitingdept != null)
                        {

                            RedirectToAction("Index", "Department");
                            return Json(new { success = true, message = "Duplicate Record" });
                        }
                        else
                        {
                            Department department = new Department
                            {
                                DepartmentName = dep.DepartmentName,
                                CreatedAt = DateTime.Now
                            };
                            db.Departments.Add(department);
                        }


                }
                else
                {
                    var department = db.Departments.Find(dep.DepartmentId);
                    if (department != null)
                    {
                        department.DepartmentName = dep.DepartmentName;
                        department.UpdatedAt = DateTime.Now;
                        db.Entry(department).State = EntityState.Modified;
                    }

                }
                db.SaveChanges();
                return Json(new { success = true });
                // return RedirectToAction("Index");
            }
                else
                {
                    return Json(new { success = false });
                }

            }
           catch(DbUpdateException ex)
            {
                var inner = ex.InnerException?.InnerException?.Message;
                throw new Exception("Save failed: " + inner);
            }

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var exitingdept = db.Departments.Where(c => c.DepartmentId == id).FirstOrDefault();
            if (exitingdept != null)
            {
                //delete single records using remove 
                db.Departments.Remove(exitingdept);
            }
            db.SaveChanges();
            return Json(new { success = true });

        }

    }
}