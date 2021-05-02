using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModel;

namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        MyEshop_DBEntities db =new MyEshop_DBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Slider()
        {
            DateTime dt=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
            return PartialView(db.Slider.Where(s=>s.IsActive&&s.StartDate<=dt&&s.EndDate>=dt));
        }

        public ActionResult VisitSite()
        {
            DateTime dtNow = DateTime.Now.Date;
            DateTime yesterday = dtNow.AddDays(-1);
            VisitSiteViewModel visit=new VisitSiteViewModel();
            visit.VisitSum = -db.SiteVisit.Count();
            visit.VisitTodey = db.SiteVisit.Count(v => v.Date == dtNow);
            visit.VisitYesterday = db.SiteVisit.Count(v => v.Date == yesterday);
            visit.Online = int.Parse(HttpContext.Application["Online"].ToString());

            return PartialView(visit);
        }

    }
}