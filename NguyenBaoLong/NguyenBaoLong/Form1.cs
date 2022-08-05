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
        ProductBUS b;
        int productId;

        public Form1()
        {
            InitializeComponent();
            b = new ProductBUS();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            gvSanPham.DataSource = b.GetProducts();
            b.GetCategories(cbLoaiSP);
            b.GetSuppliers(cbNCC);
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

                int r = b.AddProduct(p);

                switch(r)
                {
                    case -1:
                        MessageBox.Show("San pham da ton tai");
                        break;
                    case 0:
                        MessageBox.Show("Them that bai");
                        break;
                    case 1:
                        gvSanPham.DataSource = b.GetProducts();
                        MessageBox.Show("Them thanh cong");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > 0 && e.RowIndex < gvSanPham.Rows.Count)
            {
                productId = int.Parse(gvSanPham.Rows[e.RowIndex].Cells["ProductID"].Value.ToString());
                txtTenSP.Text = gvSanPham.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                txtSoLuong.Text = gvSanPham.Rows[e.RowIndex].Cells["QuantityPerUnit"].Value.ToString();
                txtDonGia.Text = gvSanPham.Rows[e.RowIndex].Cells["UnitPrice"].Value.ToString();
                cbLoaiSP.SelectedValue = gvSanPham.Rows[e.RowIndex].Cells["CategoryID"].Value.ToString();
                cbNCC.SelectedValue = gvSanPham.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                Product p = new Product();
                p.ProductName = txtTenSP.Text;
                p.SupplierID = int.Parse(cbNCC.SelectedValue.ToString());
                p.CategoryID = int.Parse(cbLoaiSP.SelectedValue.ToString());
                p.QuantityPerUnit = int.Parse(txtSoLuong.Text.ToString());
                p.UnitPrice = double.Parse(txtDonGia.Text.ToString());

                int r = b.UpdateProduct(p);

                switch (r)
                {
                    case -1:
                        MessageBox.Show("San pham da ton tai");
                        break;
                    case 0:
                        MessageBox.Show("Sua that bai");
                        break;
                    case 1:
                        gvSanPham.DataSource = b.GetProducts();
                        MessageBox.Show("Sua thanh cong");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
