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

        // View for create 
        public ActionResult Create()
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Product product)
        {
            int ncc = int.Parse(collection["NCC"]);
            int lsp = int.Parse(collection["LSP"]);
            var tenSP = collection["ProductName"];

            if(String.IsNullOrEmpty(tenSP))
            {
                ViewData["Loi"] = "Khong co ten san pham";
            }

            else
            {
                product.SupplierID = ncc;
                product.CategoryID = lsp;
                da.Products.InsertOnSubmit(product);
                da.SubmitChanges();
                return RedirectToAction("ListProducts");
            }
            return this.Create();
        }

        // View for edit
        public ActionResult Edit(int id)
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // Create
        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var tenSP = collection["ProductName"];

            if (String.IsNullOrEmpty(tenSP))
            {
                ViewData["Loi"] = "Khong co ten san pham";
            }

            else
            {
                Product product = da.Products.First(s => s.ProductID == id);
                product.ProductName = collection["ProductName"];
                product.SupplierID = int.Parse(collection["NCC"]);
                product.CategoryID = int.Parse(collection["LSP"]);
                try
                {
                    product.UnitPrice = decimal.Parse(collection["UnitPrice"]);
                    product.QuantityPerUnit = collection["QuantityPerUnit"];

                    TryUpdateModel(product);
                    da.SubmitChanges();
                    return RedirectToAction("ListProducts");
                }
                catch (Exception ex)
                {
                    ViewData["Loi"] = ex.Message;
                    return this.Edit(id);
                }               
            }
            return this.Edit(id);
        }

        // View for delete 
        public ActionResult Delete(int id)
        {
            Product p = da.Products.Where(s => s.ProductID == id).SingleOrDefault();
            return View(p);
        }

        // Delete
        [HttpPost]
        public ActionResult Delete(FormCollection collection, int id)
        {
            Product product = da.Products.First(s => s.ProductID == id);
            da.Products.DeleteOnSubmit(product);
            da.SubmitChanges();
            return RedirectToAction("ListProducts");
        }
    }
}