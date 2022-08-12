using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBHNguyenBaoLong.Models;
using System.Transactions;

namespace QLBHNguyenBaoLong.Controllers
{
    public class CartController : Controller
    {
        private NWDataContext dt = new NWDataContext();
        // GET: Cart

        public List<Cart> GetListCart()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if(carts == null)
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }
            return carts;
        }

        public ActionResult AddCart(int id)
        {
            List<Cart> carts = GetListCart();
            Cart c = carts.Find(s => s.ProductID == id);
            if (c == null)
            {
                c = new Cart(id);
                carts.Add(c);
            }
            else
            {
                c.Quantity++;
            }
            return RedirectToAction("ListCart");
        }

        public ActionResult ListCart()
        {
            List<Cart> carts = GetListCart();
            
            if (carts.Count==0)
            {
                return RedirectToAction("ListProducts", "Product");
            }
            ViewBag.CountProduct = Count();
            ViewBag.Total = Total();
            
            return View(carts);
        }

        private int Count()
        {
            int n = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                n = carts.Sum(s => s.Quantity);
            }
            return n;
        }

        private decimal Total()
        {
            decimal total = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if(carts != null)
            {
                total = carts.Sum(s => s.Total);
            }
            return total;
        }

        public ActionResult Delete(int id)
        {
            List<Cart> carts = GetListCart();
            Cart c = carts.Find(s => s.ProductID == id);

            if (c != null)
            {
                carts.RemoveAll(s => s.ProductID == id);
                return RedirectToAction("ListCart");
            }
            if(carts.Count == 0)
            {
                return RedirectToAction("ListProducts", "Product");
            }
            return RedirectToAction("ListCart");
        }
        public ActionResult OrderProduct(FormCollection collection)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    dt.Orders.InsertOnSubmit(order);
                    dt.SubmitChanges();
                    List<Cart> carts = GetListCart();
                    foreach (var item in carts)
                    {
                        Order_Detail d = new Order_Detail();
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quantity.ToString());
                        d.UnitPrice = item.UnitPrice;
                        d.Discount = 0;

                        dt.Order_Details.InsertOnSubmit(d);

                    }
                    dt.SubmitChanges();
                    tranScope.Complete();
                } 
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCart");
                }

            }
            return RedirectToAction("OrderDetailList", "Cart");
        }

        public ActionResult OrderDetailList()
        {
            var p = dt.Order_Details.OrderByDescending(s => s.OrderID).Select(s => s).ToList();
            return View(p);
        }
    }
}