using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models.ViewModel;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    //[Authorize]
    public class DesignationController : Controller
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
            var designList = db.Designations.ToList();
            return Json(new { data = designList }, JsonRequestBehavior.AllowGet);
        }

        //Dropdown list show in in _partial Designation
        [HttpGet]
        public JsonResult GetDesignation()
        {
            var designationList = db.Designations.Select(c => new
            {
                c.DesignationId,
                c.DesignationName
            }).ToList();
            return Json(designationList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return PartialView("_AddOrEditDesignation", new Designation());
            else
            {
                var emp = db.Designations.Find(id);
                return PartialView("_AddOrEditDesignation", emp);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(VMDesignation dep)
        {

            try
            {
                if(ModelState.IsValid)
                {
                    if (dep.DesignationId == 0)
                    {
                        var exitingdesign = db.Designations.Where(c => c.DesignationName == dep.DesignationName).FirstOrDefault();
                        if (exitingdesign != null)
                        {
                            RedirectToAction("Index", "Designation");
                            return Json(new { success = true, message = "Duplicate Record" });
                        }
                        else
                        {
                            Designation designation = new Designation
                            {
                                DesignationName = dep.DesignationName,
                                DesignationDescription = dep.DesignationDescription,
                                CreatedAt = DateTime.Now
                            };
                            db.Designations.Add(designation);
                        }

                    }
                    else
                    {
                        var designation = db.Designations.Find(dep.DesignationId);
                        if (designation != null)
                        {
                            designation.DesignationName = dep.DesignationName;
                            designation.DesignationDescription = dep.DesignationDescription;
                            designation.UpdatedAt = DateTime.Now;
                            db.Entry(designation).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
                
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.InnerException?.Message;
                throw new Exception("Save failed: " + inner);
            }

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var exitingdesign = db.Designations.Where(c => c.DesignationId == id).FirstOrDefault();
                if (exitingdesign != null)
                {
                    db.Designations.Remove(exitingdesign);
                }
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                return Json(new { error = false });
            }
        }
    }
}