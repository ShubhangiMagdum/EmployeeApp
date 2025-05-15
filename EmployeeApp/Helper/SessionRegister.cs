using System;
using System.Linq;
using System.Web;
using EmployeeApp.Models;

namespace EmployeeApp.Helper
{
    public static class SessionRegister
    {
 
        public static void SetSession(HttpSessionStateBase session, int employeeId, string name, string roleName,string profileImage,string emailId)
        {
            session["UserId"] = employeeId;
            session["UserName"] = name;
            session["UserRole"] = roleName;
            session["UserProfileImage"] = profileImage;
            session["UserEmail"] = emailId;

        }

        //after login : timeing clear  
        public static void ClearSession(HttpSessionStateBase session)
        {
            session.Clear();
            session.Abandon();
        }

        //is logged not Or yes 
        public static bool IsLoggedIn(HttpSessionStateBase session)
        {
            return session["UserId"] != null;
        }

        public static string GetUserRole(HttpSessionStateBase session)
        {
            return session["UserRole"]?.ToString();
        }
    }

}