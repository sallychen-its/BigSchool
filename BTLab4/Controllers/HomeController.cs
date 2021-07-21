using BTLab5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTLab5.Controllers
{
    public class HomeController : Controller
    {
        DataSQL db = new DataSQL();
        public ActionResult Index()
        {
            var upcommingCourse = db.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            var userID = User.Identity.GetUserId();
            foreach (Course item in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.LecturerId);
                item.Name = user.Name;
                if (userID != null)
                {
                    item.isLogin = true;
                    Attendance a = db.Attendances.FirstOrDefault(p => p.CourseId == item.Id && p.Attendee == userID);
                    if (a == null) item.isShowGoing = true;
                    Following find = db.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == item.LecturerId);
                    if (find == null) item.isShowFollow = true;
                }
            }
            return View(upcommingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}