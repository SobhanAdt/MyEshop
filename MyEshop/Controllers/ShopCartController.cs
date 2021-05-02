using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModel;

namespace MyEshop.Controllers
{
    public class ShopCartController : Controller
    {
        MyEshop_DBEntities db = new MyEshop_DBEntities();
        // GET: ShopCart
        public ActionResult ShowCart()
        {
            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = (List<ShopCartItem>)Session["ShopCart"];
                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.ProductTitle
                    }).Single();
                    list.Add(new ShopCartItemViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Title=product.ProductTitle,
                        ImageName=product.ImageName

                    });
                }
            }

            return PartialView(list);
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Oredr()
        {
            return PartialView(getListOrder());
        }

        List<ShowOrderViewModel> getListOrder()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.ProductTitle,
                        p.Price
                    }).Single();

                    list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Price = product.Price,
                        Title = product.ProductTitle,
                        ImageName = product.ImageName,
                        Sum = item.Count * product.Price
                    });
                }
            }

            return list;
        }

        public ActionResult CommandOrder(int id, int count)
        {
            List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;

            int index = listShop.FindIndex(p => p.ProductID == id);

            if (count==0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }

            Session["ShopCart"] = listShop;

            return PartialView("Oredr",getListOrder());
        }

        [Authorize]
        public ActionResult Payment()
        {
            int userId = db.Users.Single(u => u.UserName == User.Identity.Name).UserID;

            Orders order=new Orders()
            {
                UserID=userId,
                Date=DateTime.Now,
                IsFinaly = false
            };
            db.Orders.Add(order);

            var listDetails = getListOrder();

            foreach (var item in listDetails)
            {
                db.OrderDetails.Add(new OrderDetails()
                {
                    Count = item.Count,
                    OrderID = order.OrderID,
                    Price = item.Price,
                    ProductID = item.ProductID
                });
            }

            db.SaveChanges();

            //ToDo : Online Payment

            return null;
        }
    }
}