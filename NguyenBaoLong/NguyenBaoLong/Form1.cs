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


        private bool AddProduct(string productName, int supplierID, int categoryID, int quantityPerUnit, double unitPrice)
        {
            bool r = false;

            string query = String.Format("insert into Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice) values ({0}, {1}, {2}, {3}, {4})",
                productName, supplierID, categoryID, quantityPerUnit,unitPrice);

            SqlCommand cmd = new SqlCommand(query, conn);
 
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                r = true;
            }
            catch (Exception)
            {
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
                string productName = txtTenSP.Text.ToString();
                int supplierID = int.Parse(cbNCC.SelectedValue.ToString());
                int categoryID = int.Parse(cbLoaiSP.SelectedValue.ToString());
                int quantityPerUnit = int.Parse(txtSoLuong.Text.ToString());
                double unitPrice = double.Parse(txtDonGia.Text.ToString());
                bool r = AddProduct(productName, supplierID, categoryID, quantityPerUnit, unitPrice);

                if (r)
                {
                    MessageBox.Show("Them thanh cong");
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
