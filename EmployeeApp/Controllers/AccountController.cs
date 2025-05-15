using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EmployeeApp.Data;
using EmployeeApp.Helper;
using Microsoft.Ajax.Utilities;

namespace EmployeeApp.Controllers
{
    public class AccountController : Controller
    {
            private ApplicationDbContext db = new ApplicationDbContext();

            // GET: Login
           
            public ActionResult Index()
            {
                return View();
            }


        //    // POST: Login/LoginSubmit
        //    [HttpPost]
        //    [AllowAnonymous]
        //    public JsonResult LoginSubmit(string email, string password)
        //    {
        //    try
        //    {
        //        var user = db.Employees.FirstOrDefault(e => e.Email == email && e.PasswordHash == password);

        //        if (user != null)
        //        {
        //            string roleName = db.Roles
        //                                .Where(r => r.RoleId == user.RoleId)
        //                                .Select(r => r.RoleName)
        //                                .FirstOrDefault();

        //            // Get profile image URL (you can customize this based on your database structure)
        //            string profileImage = user.profileImg ?? "/Images/profile/default-profile.jpg";

        //            SessionRegister.SetSession(Session, user.EmployeeId, user.EmployeeName, roleName, profileImage, user.Email);
        //            //FormsAuthentication.SetAuthCookie(user.Email, false);


        //            if (roleName == "Admin")
        //                return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Admin") });

        //            if (roleName == "Employee")
        //                return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Employee") });

        //        }



        //        return Json(new { success = false, message = "Invalid credentials" });
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        var inner = ex.InnerException?.InnerException?.Message;
        //        throw new Exception("Save failed: " + inner);
        //    }



        //}

        //logout role wise 

        [HttpPost]
       // [AllowAnonymous]
        public JsonResult LoginSubmit(string email, string password)
        {
            try
            {
                var user = db.Employees.FirstOrDefault(e => e.Email == email && e.PasswordHash == password);
                if (user != null)
                {
                    var roleName = db.Roles
                                     .Where(r => r.RoleId == user.RoleId)
                                     .Select(r => r.RoleName)
                                     .FirstOrDefault();

                    var profileImage = user.profileImg ?? "/Images/profile/default-profile.jpg";

                    SessionRegister.SetSession(Session, user.EmployeeId, user.EmployeeName, roleName, profileImage, user.Email);

                    // Important: Create the auth cookie manually
                   // FormsAuthentication.SetAuthCookie(user.Email, false);

                    if (roleName == "Admin")
                        return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Admin") });

                    if (roleName == "Employee")
                       return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Employee") });

                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }

                return Json(new { success = false, message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        public ActionResult Logout()
            {
                FormsAuthentication.SignOut();
                SessionRegister.ClearSession(Session);
                return RedirectToAction("Index", "Account");
      
            }

        }
    }
