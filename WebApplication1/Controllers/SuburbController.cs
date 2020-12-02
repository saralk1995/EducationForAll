using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class SuburbController : Controller
    {
        // GET: Suburb
        public ActionResult getSuburb(String id)
        {
            Suburb db = new Suburb();
            var suburbList = db.postcodes_location.Where(x => x.state == id).Select(x => x.suburb).ToList();
            return Json(suburbList, JsonRequestBehavior.AllowGet);
        }
    }
}