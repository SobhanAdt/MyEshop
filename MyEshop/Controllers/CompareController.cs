using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModel;

namespace MyEshop.Controllers
{
    public class CompareController : Controller
    {
        MyEshop_DBEntities db=new MyEshop_DBEntities();
        // GET: Compare
        public ActionResult Index()
        {
            List<CompareItam> list = new List<CompareItam>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItam>;
            }
            List<Features> featurs=new List<Features>();
            List<Product_Features> productFeatures=new List<Product_Features>();
            foreach (var item in list)
            {
                featurs.AddRange(db.Product_Features.Where(p=>p.ProductID==item.ProductID).Select(p=>p.Features).ToList());
                productFeatures.AddRange(db.Product_Features.Where(p=>p.ProductID==item.ProductID).ToList());
            }

            ViewBag.productFeatures = productFeatures;
            ViewBag.features = featurs.Distinct().ToList();
            return View(list);
        }

        public ActionResult AddtoCampare(int id)
        {
            List<CompareItam> list=new List<CompareItam>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItam>;
            }

            if (!list.Any(p=>p.ProductID==id))
            {
                var product = db.Products.Where(p => p.ProductID == id).Select(p => new {p.ProductTitle, p.ImageName}).Single();
                list.Add(new CompareItam()
                {
                    ProductID=id,
                    Title = product.ProductTitle,
                    ImageName=product.ImageName
                });
            }

            Session["Compare"] = list;

            return PartialView("ListCompare",list);
        }

        public ActionResult ListCompare()
        {
            List<CompareItam> list = new List<CompareItam>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItam>;
            }

            return PartialView(list);
        }

        public ActionResult DeleteFromCompare(int id)
        {
            List<CompareItam> list = new List<CompareItam>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItam>;
                int index = list.FindIndex(p => p.ProductID == id);
                list.RemoveAt(index);
                Session["Compare"] = list;
            }

            return PartialView("ListCompare", list);
        }
    }
}