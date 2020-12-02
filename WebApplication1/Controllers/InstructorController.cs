using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InstructorController : Controller
    {
        [Authorize]
        // GET: Instructor
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult AddCourse()
        {
            return View();
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public ActionResult ViewRatings()
        {
            EducationForAll db = new EducationForAll();
            return View(db.Courses.ToList());
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult ViewStudents()
        {
            EducationForAll db = new EducationForAll();
            return View(db.Students.ToList());
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult InstructorProfile()
        {
            return View();
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public ActionResult AddCourse(WebApplication1.Models.Cours courseModel)
        {
            string userId = User.Identity.GetUserId();

            using (EducationForAll db = new EducationForAll())
            {
                var InstructorId = db.Instructors.Where(x => x.user_id == userId).FirstOrDefault();
                Cours course = new Cours();
                course.CourseName = courseModel.CourseName;
                course.CourseDuration = courseModel.CourseDuration;
                course.CourseStartDate = courseModel.CourseStartDate;
                course.CourseStatus = courseModel.CourseStatus;
                course.CourseFee = courseModel.CourseFee;
                course.CourseDomain = courseModel.CourseDomain;
                course.CourseTags = courseModel.CourseTags;
                course.InstructorUser_id = InstructorId.InstructorId;
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index", "Instructor");
            }
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult SendEmail()
        {
            EducationForAll db = new EducationForAll();
            MailViewModel model1 = new MailViewModel();
            model1.studentList = db.Students.Select(x => x.StudentName).ToList();
            return View(model1);
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public ActionResult SendEmail(WebApplication1.Models.MailViewModel mailModel)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                using (EducationForAll db = new EducationForAll())
                {
                    var userid = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                    string from = userid.Email;
                    string to = mailModel.sampleMail.To;
                    var studentDetails = db.Students.Where(x => x.StudentName == to).FirstOrDefault();
                    using (MailMessage mail = new MailMessage(from, studentDetails.StudentEmail))
                    {
                        mail.Subject = mailModel.sampleMail.Subject;
                        mail.Body = mailModel.sampleMail.Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        string password = mailModel.sampleMail.Password;
                        NetworkCredential networkCredential = new NetworkCredential(from, password);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = networkCredential;
                        smtp.Port = 587;
                        smtp.Send(mail);
                        ViewBag.Message = "Sent";
                        return RedirectToAction("Index", "Instructor");
                    }
                }
            }
            else
            {
                return View();
            }
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public ActionResult SendBulk()
        {
            return View();
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public ActionResult SendBulk(WebApplication1.Models.BulkMail bulkModel)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                using (EducationForAll db = new EducationForAll())
                {
                    var userid = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                    string from = userid.Email;
                    var studentEmailList = db.Students.Select(x => x.StudentEmail).ToList();
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        foreach (var email in studentEmailList)
                        {
                            mail.To.Add(email);
                        }
                        mail.Subject = bulkModel.Subject;
                        mail.Body = bulkModel.Body;
                        mail.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        string password = bulkModel.Password;
                        NetworkCredential networkCredential = new NetworkCredential(from, password);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = networkCredential;
                        smtp.Port = 587;
                        smtp.Send(mail);
                        ViewBag.Message = "Sent";
                        return RedirectToAction("Index", "Instructor");
                    }
                }
            }
            else
            {
                return View();
            }

        }
        [Authorize(Roles = "Instructor")]
        public ActionResult ViewGraph()
        {
            var dataPoints = TempData["doc"];
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult aggregateRatings()
        {
            var userId = User.Identity.GetUserId();
            EducationForAll db = new EducationForAll();
            var instructorId = db.Instructors.Where(x => x.user_id == userId).Select(x => x.InstructorId).FirstOrDefault();
            var courseList = db.Courses.Where(x => x.InstructorUser_id == instructorId).Select(x => x.CourseName).ToList();
            aggregateRatings rating = new aggregateRatings();
            rating.courseList = courseList;
            return View(rating);
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult aggregateRatings1(WebApplication1.Models.aggregateRatings agrremodel,string submitButton)
        {
            if(submitButton == "ShowRatingsStatistics")
            { 
            EducationForAll db = new EducationForAll();
            aggregateRatings rating = new aggregateRatings();
            var courseId = db.Courses.Where(x => x.CourseName == agrremodel.selectedCourse).FirstOrDefault();
            var reviewList = db.Reviews.Where(x => x.CourseId == courseId.CourseId).Select(x => x.ReviewStar).ToList();
            int highestRatings = reviewList.Max();
            int lowestRatings = reviewList.Min();
            double averageRatings = reviewList.Average();
            rating.highestRating = highestRatings;
            rating.lowestRating = lowestRatings;
            rating.averagerating = Math.Round(averageRatings);
            rating.selectedCourse = agrremodel.selectedCourse;
            ViewBag.rating = rating;
            return View();
             }
            else
            {
                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
                int count4 = 0;
                int count5 = 0;
                EducationForAll db = new EducationForAll();
                var courseId = db.Courses.Where(x => x.CourseName == agrremodel.selectedCourse).FirstOrDefault();
                var reviewList = db.Reviews.Where(x=>x.CourseId == courseId.CourseId).Select(x => x.ReviewStar).ToList();
                foreach (var review in reviewList)
                {
                    if (review == 1)
                        count1++;
                    if (review == 2)
                        count2++;
                    if (review == 3)
                        count3++;
                    if (review == 4)
                        count4++;
                    if (review == 5)
                        count5++;
                }
                List<DataPoint> dataPoints = new List<DataPoint>();

                dataPoints.Add(new DataPoint("Rating 1", count1));
                dataPoints.Add(new DataPoint("Rating 2", count2));
                dataPoints.Add(new DataPoint("Rating 3", count3));
                dataPoints.Add(new DataPoint("Rating 4", count4));
                dataPoints.Add(new DataPoint("Rating 5", count5));
                TempData["doc"] = dataPoints;
                return RedirectToAction("ViewGraph", "Instructor");
            }
        }
        [Authorize(Roles = "Instructor")]
        public FileResult createPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            PdfPTable tableLayout = new PdfPTable(5);
            doc.SetMargins(0f, 0f, 0f, 0f);
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            doc.Add(Add_Content_To_PDF(tableLayout));
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 24, 45, 35, 50 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  
            EducationForAll db = new EducationForAll();
            var studentDetails = db.Students.ToList();

            tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new Font(Font.FontFamily.HELVETICA, 13, 1)))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            ////Add header  
            AddCellToHeader(tableLayout, "StudentName");
            AddCellToHeader(tableLayout, "StudentEmail");
            AddCellToHeader(tableLayout, "StudentQualification");
            AddCellToHeader(tableLayout, "StudentPhoneNo");
            AddCellToHeader(tableLayout, "StudentLocation");

            ////Add body  
            foreach (var student in studentDetails)
            {

                AddCellToBody(tableLayout, student.StudentName);
                AddCellToBody(tableLayout, student.StudentEmail);
                AddCellToBody(tableLayout, student.StudentQualification);
                AddCellToBody(tableLayout, student.StudentPhoneNo);
                AddCellToBody(tableLayout, student.StudentCity);

            }

            return tableLayout;
        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 3,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
            });
        }
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 3
            });
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public ActionResult ViewMap(int id)
        {
            EducationForAll db = new EducationForAll();
            var studentSuburb = db.Students.Where(x => x.StudentId == id).Select(x => x.StudentSuburb).FirstOrDefault();
            var studentCity = db.Students.Where(x => x.StudentId == id).Select(x => x.StudentCity).FirstOrDefault();
            var userId = User.Identity.GetUserId();
            var instructorId = db.Instructors.Where(x => x.user_id == userId).Select(x => x.InstructorId).FirstOrDefault();
            var instructorSuburb = db.Instructors.Where(x => x.InstructorId == instructorId).Select(x => x.InstructorSuburb).FirstOrDefault();
            var instructorCity = db.Instructors.Where(x => x.InstructorId == instructorId).Select(x => x.InstructorLocation).FirstOrDefault();
            Suburb db1 = new Suburb();
            var studentLat = db1.postcodes_location.Where(x => x.state == studentCity && x.suburb == studentSuburb).Select(x => x.latitude).FirstOrDefault();
            var studentLon = db1.postcodes_location.Where(x => x.suburb == studentSuburb && x.state == studentCity).Select(x => x.longitude).FirstOrDefault();

            var instructorLat = db1.postcodes_location.Where(x => x.state == instructorCity && x.suburb == instructorSuburb).Select(x => x.latitude).FirstOrDefault();
            var instructorLon = db1.postcodes_location.Where(x => x.suburb == instructorSuburb && x.state == instructorCity).Select(x => x.longitude).FirstOrDefault();

            ViewBag.studentLatitude = studentLat;
            ViewBag.studentLongitude = studentLon;
            ViewBag.instructorLatitude = instructorLat;
            ViewBag.instructorLongitude = instructorLon;
            return View();
        }
        public ActionResult addEvent()
        {
            return View();
        }
        public JsonResult GetEvents()
        {
            using (Model5 dc = new Model5())
            {
                var events = dc.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (Model5 dc = new Model5())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (Model5 dc = new Model5())
            {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}