using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenBaoLong
{
    class ProductDAL
    {
        SqlConnection conn;

        public ProductDAL()
        {
            ConnectDatabase();
        }

        private void ConnectDatabase()
        {
            string conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            conn = new SqlConnection(conStr);
        }

        public DataTable GetProducts()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            String query = "SELECT * FROM Products";
            da = new SqlDataAdapter(query, conn);
            da.Fill(dt); 
            

            return dt;
        }

        public DataTable GetCategories()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            String query = "select CategoryID, CategoryName from Categories";
            da = new SqlDataAdapter(query, conn);
            da.Fill(dt);

            return dt;
        }

        public DataTable GetSuppliers()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            String query = "select SupplierID, CompanyName from Suppliers";
            da = new SqlDataAdapter(query, conn);
            da.Fill(dt);       

            return dt;
        }

        public bool CheckProduct(Product p)
        {
            bool r = false;
            int n = 0;

            string query = String.Format("select count(ProductID) from Products where ProductName=N'{0}'", p.ProductName);

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                n = int.Parse(cmd.ExecuteScalar().ToString());
                r = n > 0 ? true : false;
            }
            catch (Exception ex)
            {
                r = false;
            }
            finally
            {
                conn.Close();
            }

            return r;
        }

        public bool CheckProductByID(int id)
        {
            bool r = false;
            int n = 0;

            string query = String.Format("select count(ProductID) from Products where ProductID=N'{0}'", id);

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                n = int.Parse(cmd.ExecuteScalar().ToString());
                r = n > 0 ? true : false;
            }
            catch (Exception ex)
            {
                r = false;
            }
            finally
            {
                conn.Close();
            }

            return r;
        }

        public int AddProduct(Product p)
        {
            int r = 0;

            string query = String.Format("insert into Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice) values (N'{0}', {1}, {2}, {3}, {4})",
                p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice);

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                r = int.Parse(cmd.ExecuteNonQuery().ToString());
                r = r > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                r = 0;
            }
            finally
            {
                conn.Close();
            }

            return r;
        }

        public int UpdateProduct(Product p)
        {
            int r = 0;

            string query = String.Format("update Products SET ProductName = '{0}', "+
                                                            "SupplierID = '{1}', " +
                                                            "CategoryID = '{2}', " +
                                                            "QuantityPerUnit = '{3}', "+
                                                            "UnitPrice = '{4}'" +
                                                            "where ProductID ={5}",
                                                            p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice);

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                r = int.Parse(cmd.ExecuteNonQuery().ToString());
                r = r > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                r = 0;
            }
            finally
            {
                conn.Close();
            }

            return r;
        }
    }
}
