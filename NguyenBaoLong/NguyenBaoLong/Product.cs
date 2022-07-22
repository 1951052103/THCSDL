using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenBaoLong
{
    internal class Product
    {
        private string productName;
        private int supplierID;
        private int categoryID;
        private int quantityPerUnit;
        private double unitPrice;

        public Product()
        {
        }

        public Product(string productName, int supplierID, int categoryID, int quantityPerUnit, double unitPrice)
        {
            this.productName = productName;
            this.supplierID = supplierID;
            this.categoryID = categoryID;
            this.quantityPerUnit = quantityPerUnit;
            this.unitPrice = unitPrice;
        }

        public string ProductName { get => productName; set => productName = value; }
        public int SupplierID { get => supplierID; set => supplierID = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public int QuantityPerUnit { get => quantityPerUnit; set => quantityPerUnit = value; }
        public double UnitPrice { get => unitPrice; set => unitPrice = value; }
    }
}
