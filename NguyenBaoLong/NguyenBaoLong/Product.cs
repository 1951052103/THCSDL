using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenBaoLong
{
    internal class Product
    {
        private int productID;
        private string productName;
        private int supplierID;
        private int categoryID;
        private int quantityPerUnit;
        private double unitPrice;

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

        public int SupplierID
        {
            get
            {
                return supplierID;
            }

            set
            {
                supplierID = value;
            }
        }

        public int CategoryID
        {
            get
            {
                return categoryID;
            }

            set
            {
                categoryID = value;
            }
        }

        public int QuantityPerUnit
        {
            get
            {
                return quantityPerUnit;
            }

            set
            {
                quantityPerUnit = value;
            }
        }

        public double UnitPrice
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

        public Product()
        {
        }

        public Product(string productName, int supplierID, int categoryID, int quantityPerUnit, double unitPrice)
        {
            this.ProductName = productName;
            this.SupplierID = supplierID;
            this.CategoryID = categoryID;
            this.QuantityPerUnit = quantityPerUnit;
            this.UnitPrice = unitPrice;
        }

        
    }
}
