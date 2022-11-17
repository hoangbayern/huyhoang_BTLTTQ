using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finnalBTL
{
    public partial class frmQLNCC : Form
    {
        KetNoiDuLieu kn = new KetNoiDuLieu();
        public frmQLNCC()
        {
            InitializeComponent();
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }
        private void Load_dulieu()
        {
            try
            {
                string sql = "select * from DSNhaCungCap";
                DataTable dt = kn.TaoBang(sql);

                dataNCC.DataSource = dt;
                //databindings
                txtMaNCC.DataBindings.Clear();
                txtTenNCC.DataBindings.Clear();
                txtDiaChi.DataBindings.Clear();
                txtSDT.DataBindings.Clear();
                //hiển thị dữ liệu vào các ô texdtBox, comboBox
                txtMaNCC.DataBindings.Add("Text", dt, "MaNCC");
                txtTenNCC.DataBindings.Add("Text", dt, "TenNCC");
                txtDiaChi.DataBindings.Add("Text", dt, "DiaChi");
                txtSDT.DataBindings.Add("Text", dt, "SDT");
            }
            catch
            {
                MessageBox.Show("Lỗi kết nối tới bảng DSNhaCungCap", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public string TaoMaNCC()
        {
            string ma = "";
            string taoma = "select MaNCC from DSNhaCungCap where MaNCC=(select max(MaNCC) from DSNhaCungCap)";
            DataTable dt = kn.TaoBang(taoma);
            //nếu trong bảng chưa có mã hàng thì tạo mới
            if (dt.Rows.Count <= 0)
            {
                ma = ma + Convert.ToString("NCC001");
            }
            else//Nếu đã có mã trong bảng thì tăng mã hàng lên
            {
                //Lấy 3 ký tự cuối của mã chuyển thành int
                int k = Convert.ToInt32(dt.Rows[0]["MaNCC"].ToString().Substring(dt.Rows[0]["MaNCC"].ToString().Length - 3, 3));
                //sau đó tăng thêm 1 đơn vị
                k = k + 1;
                if (k < 10)
                {
                    ma = ma + "NCC00" + k.ToString();
                }
                else if (k < 100)
                {
                    ma = ma + "NCC0" + k.ToString();
                }
            }
            return ma;
        }

        private void frmQLNCC_Load(object sender, EventArgs e)
        {
            Load_dulieu();
            //các nút button Them, Sua, Xoa, Luu bị ẩn => chọn làm mới nút Them sẽ hiển thị
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            btnSave.Enabled = false;
            btnRefresh.Focus();
            //các ô bị ẩn khi chưa nhấn làm mới
            txtTenNCC.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSDT.Enabled = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataNCC.Enabled = false;
            //các nút Sua, Xoa bị ẩn
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            //hiển thị các ô để nhập
            txtTenNCC.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSDT.Enabled = true;
            //xóa tất cả dữ liệu có sẵn trong textBox
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            //tạo mã NCC tự động
            txtMaNCC.Text = TaoMaNCC();
        }

        private void dataNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //khi chọn hàng các button bị ẩn
            btnAdd.Enabled = false;
            //các button Sua, Xoa hiện lên
            btnDelete.Enabled = true;
            btnPrepair.Enabled = true;
            //các ô textbox bị ẩn
            txtTenNCC.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSDT.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (txtMaNCC.Text == "" || txtTenNCC.Text == "" || txtDiaChi.Text == "" || txtSDT.Text == "")
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string them = "insert into DSNhaCungCap values(N'" + txtMaNCC.Text + "',N'" + txtTenNCC.Text + "', N'" + txtDiaChi.Text + "', N'" + txtSDT.Text + "')";
                kn.TruyVan(them);
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_dulieu();
                dataNCC.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnRefresh.Focus();
            string xoa = "delete from DSNhaCungCap where MaNCC='" + txtMaNCC.Text + "'";
            DialogResult kq = MessageBox.Show("Xóa nhà cung cấp này?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                kn.TruyVan(xoa);
                MessageBox.Show("Xóa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Load_dulieu();
        }

        private void btnPrepair_Click(object sender, EventArgs e)
        {
            //nút button Xoa bị ẩn
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            btnRefresh.Enabled = false;
            //mã nhà cung cấp ẩn đi
            txtMaNCC.Enabled = false;
            //các ô textBox hiện lên để sửa
            txtTenNCC.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSDT.Enabled = true;
            //nút button lưu hiện lên để lưu lại
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string luu = "update DSNhaCungCap set MaNCC=N'" + txtMaNCC.Text + "', TenNCC=N'" + txtTenNCC.Text + "', DiaChi=N'" + txtDiaChi.Text + "', SDT='" + txtSDT.Text + "' where MaNCC='" + txtMaNCC.Text + "'";
            kn.TruyVan(luu);
            MessageBox.Show("Sửa thông tin thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Load_dulieu();
            //lưu dữ liệu xong, nút button Luu ẩn đi
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
            btnRefresh.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string timkiem = "select * from DSNhaCungCap where MaNCC='" + txtSearchNCC.Text.Trim() + "' or TenNCC like '%"+ txtSearchNCC.Text.Trim() +"%'";
            DataTable dt = kn.TaoBang(timkiem);
            if (dt.Rows.Count > 0)
            {
                dataNCC.DataSource = dt;
                //databinding
                txtMaNCC.DataBindings.Clear();
                txtTenNCC.DataBindings.Clear();
                txtDiaChi.DataBindings.Clear();
                txtSDT.DataBindings.Clear();
                //hiển thị dữ liệu vào các ô textBox
                txtMaNCC.DataBindings.Add("Text", dt, "MaNCC");
                txtTenNCC.DataBindings.Add("Text", dt, "TenNCC");
                txtDiaChi.DataBindings.Add("Text", dt, "DiaChi");
                txtSDT.DataBindings.Add("Text", dt, "SDT");
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearchNCC.Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát không ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) this.Close();

        }

        private void txtSearchNCC_Click(object sender, EventArgs e)
        {
            Load_dulieu();
            txtSearchNCC.Clear();
        }
    }
}
