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
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            var UserID = User.Identity.GetUserId();
            if (UserID == null) return BadRequest("Please login first!");
            if (UserID == follow.FolloweeId) return Ok("cannot"); ;

            DataSQL db = new DataSQL();
            Following find = db.Followings.FirstOrDefault(p => p.FollowerId == UserID && p.FolloweeId == follow.FolloweeId);
            if (find != null)
            {
                // UnFollow
                db.Followings.Remove(find);
                db.SaveChanges();
                return Ok("unfollow");
            }
            // Follow
            follow.FollowerId = UserID;
            db.Followings.Add(follow);
            db.SaveChanges();
            return Ok("follow");
        }
    }
}
