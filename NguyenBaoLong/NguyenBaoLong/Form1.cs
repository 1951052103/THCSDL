using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace NguyenBaoLong
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region GlobalVariable
        SqlConnection conn;

        #endregion

        private void ConnectDatabase()
        {
            string conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            conn = new SqlConnection(conStr);
        }

        private DataTable ListProducts()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da;
                String query = "SELECT * FROM Products";
                da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return dt;
            }            return dt;
        }

        private DataTable ListCategories()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da;
                String query = "select CategoryID, CategoryName from Categories";
                da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return dt;
            }            return dt;
        }

        private DataTable ListSuppliers()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da;
                String query = "select SupplierID, CompanyName from Suppliers";
                da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return dt;
            }            return dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectDatabase();
            gvSanPham.DataSource = ListProducts();

            cbLoaiSP.DataSource = ListCategories();
            cbLoaiSP.DisplayMember = "CategoryName";
            cbLoaiSP.ValueMember = "CategoryID";

            cbNCC.DataSource = ListSuppliers();
            cbNCC.DisplayMember = "CompanyName";
            cbNCC.ValueMember = "SupplierID";
        }


        private bool AddProduct(Product p)
        {
            bool r = false;

            string query = String.Format("insert into Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice) values (N'{0}', {1}, {2}, {3}, {4})",
                p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice);

            SqlCommand cmd = new SqlCommand(query, conn);
 
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                r = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                r = false;
            }
            finally
            {
                conn.Close();
            }

            return r;
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                Product p = new Product();
                p.ProductName = txtTenSP.Text;
                p.SupplierID = int.Parse(cbNCC.SelectedValue.ToString());
                p.CategoryID = int.Parse(cbLoaiSP.SelectedValue.ToString());
                p.QuantityPerUnit = int.Parse(txtSoLuong.Text.ToString());
                p.UnitPrice = double.Parse(txtDonGia.Text.ToString());

                bool r = AddProduct(p);

                if (r)
                {
                    MessageBox.Show("Them thanh cong");
                    gvSanPham.DataSource = ListProducts();
                }
                else
                {
                    MessageBox.Show("Them that bai");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
