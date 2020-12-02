using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        [Authorize(Roles = "Student")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult EnrolledCourse()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult EnrolCourse()
        {
            EducationForAll db = new EducationForAll();
            return View(db.Courses.ToList());
        }
        [Authorize(Roles = "Student")]
        public ActionResult EnrolCourse1(int id)
        {
            string userId = User.Identity.GetUserId();
            using (EducationForAll db = new EducationForAll())
            {
                var studentId = db.Students.Where(x => x.user_id == userId).FirstOrDefault();
                Enrolment er = new Enrolment();
                er.CourseId = id;
                er.StudentId = studentId.StudentId;
                er.StudentCourseStartDate = DateTime.Today;
                er.PercentageCourseCompleted = 0;
                er.CourseDurationLeft = "0";
                db.Enrolments.Add(er);
                db.SaveChanges();
            }
            return RedirectToAction("index", "Student");
        }
        [Authorize(Roles = "Student")]
        public ActionResult RatingCourse()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult StudentProfile()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult addReview()
        {
            EducationForAll db = new EducationForAll();
            ReviewViewModel model = new ReviewViewModel();
            model.courseList = db.Courses.Select(x => x.CourseName).ToList();
            return View(model);
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult addReview(WebApplication1.Models.ReviewViewModel reviewModel)
        {
            EducationForAll db = new EducationForAll();
            var userId = User.Identity.GetUserId();
            var userid = db.Students.Where(x => x.user_id == userId).FirstOrDefault();
            var courseDetails = db.Courses.Where(x => x.CourseName == reviewModel.selectedCourse).FirstOrDefault();
            Review review = new Review();
            review.ReviewComment = reviewModel.review.ReviewComment;
            review.ReviewStar = reviewModel.review.ReviewStar;
            review.StudentId = userid.StudentId;
            review.CourseId = courseDetails.CourseId;
            review.ReviewDate = DateTime.Now;
            db.Reviews.Add(review);
            db.SaveChanges();
            return RedirectToAction("index", "Student");
        }
    }

}