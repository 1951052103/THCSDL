using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBHNguyenBaoLong.Models;

namespace QLBHNguyenBaoLong.Controllers
{
    public class ProductController : Controller
    {
        NWDataContext da = new NWDataContext();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        // ListProducts
        public ActionResult ListProducts()
        {
            List<Product> ds = da.Products.Select(s => s).ToList();
            return View(ds);
        }

        // Details
        public ActionResult Details(int id)
        {
            Product p = da.Products.Where(s => s.ProductID == id).SingleOrDefault();
            return View(p);
        }

    }
}