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
using System.IO;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EmployeeApp.Helper;
using System.Web.UI.WebControls;
using System.Drawing;

namespace EmployeeApp.Controllers
{

    public class EmployeeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var dataList = (from employee in db.Employees
                            join department in db.Departments
                            on employee.DepartmentId equals department.DepartmentId
                            join designation in db.Designations
                            on employee.DesignationId equals designation.DesignationId
                            join roles in db.Roles
                            on employee.RoleId equals roles.RoleId
                            where employee.RoleId == 2
                            select new
                            {
                                employeeId = employee.EmployeeId,
                                employeeName = employee.EmployeeName,
                                email = employee.Email,
                                contactNo = employee.ContactNo,
                                passward = employee.PasswordHash,
                                designationName = designation.DesignationName,
                                departmentName = department.DepartmentName,
                                ProfileImg = employee.profileImg

                            }).ToList();

            return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);
        }

        //Dropdown list show in Employee
        [HttpGet]
        public JsonResult GetEmployee()
        {
            var employeeList = db.Employees.Select(c => new
            {
                c.EmployeeId,
                c.EmployeeName
            }).ToList();
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
           return View(new VMEmployee());
       
        }


        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return PartialView("_AddOrEdit", new Employee());
            else
            {
                var emp = db.Employees.Find(id);
                return PartialView("_AddOrEdit", emp);
            }
        }


       
        [HttpPost]
        public ActionResult Create(VMEmployee emp, HttpPostedFileBase file)
        {
            try
            {
               
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string folderPath = Server.MapPath("~/Images/profile/");

                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string filePath = Path.Combine(folderPath, fileName);
                        file.SaveAs(filePath);

                        emp.profileImg = fileName;
                    }

                    var newEmp = new Employee
                    {
                        EmployeeName = emp.EmployeeName,
                        Email = emp.Email,
                        ContactNo = emp.ContactNo,
                        PasswordHash = emp.PasswordHash,
                        DepartmentId = emp.DepartmentId,
                        DesignationId = emp.DesignationId,
                        profileImg = emp.profileImg,
                        RoleId = 2,
                        CreatedAt = DateTime.Now
                    };

                    db.Employees.Add(newEmp);
                    db.SaveChanges();

                TempData["Success"] = "Employee created successfully!";
                return new HttpStatusCodeResult(200);
                // return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error saving employee: " + ex.Message;
                return new HttpStatusCodeResult(500);

            }
        }


        public ActionResult Edit(int id)
        {
            var employee = db.Employees.FirstOrDefault(b => b.EmployeeId == id);
         
            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.ImgPath = employee.profileImg;
            ViewData["ExistingDepartmentId"] = employee?.DepartmentId;
            ViewData["ExistingDesignationId"] = employee?.DesignationId;

            return PartialView("_AddOrEdit", employee);
        }


        [HttpPost]
        public async Task<ActionResult> EditEmployee(int id, Employee employee, HttpPostedFileBase profileImg)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Invalid data provided.";
                    return RedirectToAction("Edit", new { id });
                    //return Json(new { success = false, message = "Invalid data provided." });
                }

                var existingEmployee = await db.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    TempData["Error"] = "Employee not found.";
                    return RedirectToAction("Index");
                    //return Json(new { success = false, message = "Employee not found." });
                }

                bool isUpdated = false;

                if (!string.Equals(existingEmployee.EmployeeName, employee.EmployeeName, StringComparison.OrdinalIgnoreCase))
                {
                    existingEmployee.EmployeeName = employee.EmployeeName;
                    isUpdated = true;
                }

                if (!string.Equals(existingEmployee.ContactNo, employee.ContactNo, StringComparison.OrdinalIgnoreCase))
                {
                    existingEmployee.ContactNo = employee.ContactNo;
                    isUpdated = true;
                }

                if (!string.Equals(existingEmployee.Email, employee.Email, StringComparison.OrdinalIgnoreCase))
                {
                    existingEmployee.Email = employee.Email;
                    isUpdated = true;
                }

                if (!string.Equals(existingEmployee.PasswordHash, employee.PasswordHash, StringComparison.OrdinalIgnoreCase))
                {
                    existingEmployee.PasswordHash = employee.PasswordHash;
                    isUpdated = true;
                }

                if (existingEmployee.DepartmentId != employee.DepartmentId)
                {
                    existingEmployee.DepartmentId = employee.DepartmentId;
                    isUpdated = true;
                }

                if (existingEmployee.DesignationId != employee.DesignationId)
                {
                    existingEmployee.DesignationId = employee.DesignationId;
                    isUpdated = true;
                }

                string uploadsFolder = Server.MapPath("~/Images/profile/");

                if (profileImg != null)
                {
                    string fileName = Path.GetFileName(profileImg.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    if (!string.IsNullOrEmpty(existingEmployee.profileImg))
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, existingEmployee.profileImg);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    profileImg.SaveAs(filePath);
                    existingEmployee.profileImg = fileName;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    existingEmployee.UpdatedAt = DateTime.Now;
                    db.Entry(existingEmployee).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["Success"] = "Employee updated successfully.";

                    // return Json(new { success = true });
                }
                else
                {
                    TempData["Info"] = "No changes detected.";

                    //return Json(new { success = false, message = "No changes detected." });
                }
                return RedirectToAction("Edit", new { id });

            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.InnerException?.Message;
                TempData["Error"] = "Save failed: " + inner;
                return RedirectToAction("Edit", new { id = id });
                // var inner = ex.InnerException?.InnerException?.Message;
                //return Json(new { success = false, message = "Save failed: " + inner });
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var exitingemp = db.Employees.Where(c => c.EmployeeId == id).FirstOrDefault();
            string uploadsFolder = Server.MapPath("~/Images/profile/");
            // Check if Logo Exists and Delete
            if (!string.IsNullOrEmpty(exitingemp.profileImg))
            {

                string oldFilePath = Path.Combine(uploadsFolder, exitingemp.profileImg);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

            }
            if (exitingemp != null)
            {
                db.Employees.Remove(exitingemp);
            }
            db.SaveChanges();
            return Json(new { success = true });

        }


        //after login dahboard page display
        public ActionResult Dashboard()
        {
            if (SessionRegister.GetUserRole(Session) != "Employee")
                return RedirectToAction("Index", "Account");

            return View();
        }

        public ActionResult Profile()
       
        {
            if (Session["UserId"] != null)
            {
                int empId = Convert.ToInt32(Session["UserId"]);

                var dataList = (from employee in db.Employees
                                join department in db.Departments
                                on employee.DepartmentId equals department.DepartmentId
                                join designation in db.Designations
                                on employee.DesignationId equals designation.DesignationId
                                join roles in db.Roles
                                on employee.RoleId equals roles.RoleId
                                where employee.EmployeeId == empId
                                select new VMEmployee
                                {
                                    EmployeeId = employee.EmployeeId,
                                    EmployeeName = employee.EmployeeName,
                                    Email = employee.Email,
                                    ContactNo = employee.ContactNo,
                                    DepartmentId=employee.DepartmentId,
                                    DesignationId=employee.DesignationId,
                                    DepartmentName=department.DepartmentName,
                                    DesignationName=designation.DesignationName,
                                    PasswordHash = employee.PasswordHash,
                                    profileImg = employee.profileImg

                                }).FirstOrDefault();

                //  return Json(new { data = dataList }, JsonRequestBehavior.AllowGet);

                return View(dataList);
            }
            return RedirectToAction("Index","Account");

        }

        [HttpGet]
        public ActionResult EmployeePieChart()
        {
            var chartData = (from emp in db.Employees
                             join dept in db.Departments on emp.DepartmentId equals dept.DepartmentId
                             join desig in db.Designations on emp.DesignationId equals desig.DesignationId
                             group emp by new { dept.DepartmentName, desig.DesignationName } into g
                             select new
                             {
                                 Department = g.Key.DepartmentName,
                                 Designation = g.Key.DesignationName,
                                 EmployeeCount = g.Count()
                             }).ToList();

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeCount()
        {
            int totalEmployees = db.Employees.Count();
            //ViewBag.TotalEmployees = totalEmployees;
            return Json(totalEmployees, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Update(VMEmployee vmemployee, HttpPostedFileBase profileImg)
        {
            var editemployee = db.Employees.Find(vmemployee.EmployeeId);
            if (editemployee != null)
            {

                editemployee.EmployeeName = vmemployee.EmployeeName;
                editemployee.Email=vmemployee.Email;
                editemployee.ContactNo= vmemployee.ContactNo;
                editemployee.DepartmentId = vmemployee.DepartmentId;
                editemployee.DesignationId=vmemployee.DesignationId;

                string uploadsFolder = Server.MapPath("~/Images/profile/");

                // Handle file upload only if a new file is provided
                if (vmemployee.profileImg != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(editemployee.profileImg))
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, editemployee.profileImg);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Upload new logo
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    //Extract Image File Name.

                    string fileName = profileImg.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    profileImg.SaveAs(filePath);

                    // Set new logo filename
                    editemployee.profileImg = fileName;
                }

                db.Entry(editemployee).State = EntityState.Modified;
            }
                db.SaveChanges();

            return RedirectToAction("Profile","Employee");
        }


        //load partial view page 
        [HttpGet]
        public ActionResult LoadChangePasswordPartial()
        {
            var model = new VMChangePassword();
            return PartialView("~/Views/Employee/_Changepassward.cshtml", model);
        }


        [HttpPost]
        //changes password as employee id wise 
        public JsonResult ChangePassword(VMChangePassword model)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);

                var user = db.Employees.FirstOrDefault(e => e.EmployeeId == userId);

                if (user != null)
                {
                    if(user.PasswordHash == model.CurrentPassword)
                    {
                        user.PasswordHash = model.NewPassword;
                        db.SaveChanges();
                        ViewBag.Message = "Password changed successfully!";
                    }
                    else
                    {
                        ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "user not found.");
                }
            }

            return Json(new { success = true });
        }


        //Display recent salary slip data
        [HttpGet]
        public JsonResult RecentSalarySlip()
        {
            var dataList = (from employee in db.Employees
                            join salary in db.Salarys
                            on employee.EmployeeId equals salary.EmployeeId
                            join slip in db.salarySlips
                            on salary.SalaryId equals slip.SalaryId
                            select new
                            {
                               employee.EmployeeName,
                                slip.GeneratedDate,
                                slip.NetSalary
                            }).ToList(); 

            return Json(dataList, JsonRequestBehavior.AllowGet);

        }
    }
}