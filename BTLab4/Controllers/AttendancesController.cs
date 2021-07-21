using BTLab5.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTLab4.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userID = User.Identity.GetUserId();
            DataSQL db = new DataSQL();
            if (db.Attendances.Any(p=>p.Attendee == userID && p.CourseId == attendanceDto.Id))
            {
                Attendance a = db.Attendances.FirstOrDefault(p => p.CourseId == attendanceDto.Id);
                db.Attendances.Remove(a);
                db.SaveChanges();
                return Ok("cancel");
            }
            var attendance = new Attendance() { CourseId = attendanceDto.Id, Attendee = User.Identity.GetUserId() };
            db.Attendances.Add(attendance);
            db.SaveChanges();
            return Ok("going");
        }
    }
}
