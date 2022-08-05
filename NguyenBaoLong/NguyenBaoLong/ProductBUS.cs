using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenBaoLong
{
    class ProductBUS
    {
        ProductDAL d;

        public ProductBUS()
        {
            d = new ProductDAL();
        }

        public DataTable GetProducts()
        {
            return d.GetProducts();
        }

        public DataTable GetCategories(ComboBox c)
        {
            c.DataSource = d.GetCategories();
            c.DisplayMember = "CategoryName";
            c.ValueMember = "CategoryID";

            return d.GetProducts();
        }

        public DataTable GetSuppliers(ComboBox c)
        {
            c.DataSource = d.GetSuppliers();
            c.DisplayMember = "CompanyName";
            c.ValueMember = "SupplierID";

            return d.GetProducts();
        }

        public int AddProduct(Product p)
        {
            int r = -1;
            if(!d.CheckProduct(p))
            {
                r = d.AddProduct(p);
            }
            return r;
        }

        public int UpdateProduct(Product p)
        {
            int r = -1;
            if (d.CheckProductByID(p.ProductID))
            {
                r = d.UpdateProduct(p);
            }
            return r;
        }
    }
}
