using System;
using System.Linq;
using System.Web;
using EmployeeApp.Helper;
using System.Web.Mvc;

namespace EmployeeApp.Controllers
{

    public class AdminController : Controller
    {
       // [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {
            
            if (SessionRegister.GetUserRole(Session) != "Admin")
               return RedirectToAction("Index", "Account");

            return View();
        }
    }
}