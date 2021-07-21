using BTLab5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTLab5.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        DataSQL db = new DataSQL();

        [HttpGet]
        public ActionResult Create()
        {
            Course obj = new Course();
            obj.listCategory = db.Categories.ToList();
            return View(obj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course obj)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            obj.LecturerId = user.Id;
            obj.DateTime = DateTime.Parse(obj.DateTime.ToString("yyyy/MM/dd HH:mm:ss"));
            obj.Name = user.Name;
            db.Courses.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = db.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var item in listAttendances)
            {
                Course objCourse = item.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var courses = db.Courses.Where(p => p.LecturerId == currentUser.Id && p.DateTime > DateTime.Now).ToList();
            foreach (var item in courses)
            {
                item.Name = currentUser.Name;
            }
            return View(courses);
        }

        public ActionResult DeleteCourse(string id)
        {
            int ids = Int32.Parse(id);
            List<Attendance> list = db.Attendances.Where(p => p.CourseId == ids).ToList();
            foreach (var item in list)
            {
                db.Attendances.Remove(item);
            }
            db.SaveChanges();

            Course haha = db.Courses.FirstOrDefault(p => p.Id == ids);

            db.Courses.Remove(haha);
            db.SaveChanges();
            return RedirectToAction("Mine", "Courses");
        }

        public ActionResult EditCourse(string id)
        {
            int ids = Int32.Parse(id);
            ViewBag.listCate = db.Categories.ToList();
            Course haha = db.Courses.FirstOrDefault(p => p.Id == ids);
            return View(haha);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Mine", "Courses");
            }
            return View(course);
        }
        public ActionResult LectrureIamGoing()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listFollwee = db.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();
            var listAttendances = db.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course course1 = course.Course;
                        course1.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(course1.LecturerId).Name;
                        courses.Add(course1);

                    }
                }
            }
            return View(courses);
        }

    }
}