using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBHNguyenBaoLong.Models
{
    public class Cart
    {
        private NWDataContext dt = new NWDataContext();
        private int productID;
        private string productName;
        private decimal unitPrice;
        private int quantity;
        private decimal total;

        public int ProductID
        {
            get
            {
                return productID;
            }

            set
            {
                productID = value;
            }
        }

        public string ProductName
        {
            get
            {
                return productName;
            }

            set
            {
                productName = value;
            }
        }

        public decimal UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
            }
        }

        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        public decimal Total
        {
            get
            {
                return UnitPrice * Quantity;
            }
        }

        public Cart(int productID)
        {
            this.ProductID = productID;
            Product p = dt.Products.Single(n => n.ProductID == productID);
            ProductName = p.ProductName;
            UnitPrice = (decimal)p.UnitPrice;
            Quantity = 1;
        }
        
    }
}