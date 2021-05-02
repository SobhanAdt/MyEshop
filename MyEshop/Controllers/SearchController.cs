using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Controllers
{
    public class SearchController : Controller
    {
        MyEshop_DBEntities db=new MyEshop_DBEntities();
        // GET: Search
        public ActionResult Index(string q)
        {
            List<Products> list=new List<Products>();
            list.AddRange(db.Product_Tags.Where(t=>t.Tag==q).Select(t=>t.Products).ToList());
            list.AddRange(db.Products.Where(p=>p.ProductTitle.Contains(q)||p.ShortDescription.Contains(q)||p.Text.Contains(q)).ToList());

            ViewBag.search = q;
            return View(list.Distinct());
        }
    }
}