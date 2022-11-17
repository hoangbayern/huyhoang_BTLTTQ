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
    public partial class frmQLNV : Form
    {
        KetNoiDuLieu kn = new KetNoiDuLieu();
        public frmQLNV()
        {
            InitializeComponent();
        }
        private void Load_dulieu()
        {
            try
            {
                string sql = "select * from DSNhanVien";
                DataTable dt = kn.TaoBang(sql);
                dataNV.DataSource = dt;
                List<string> ChucVu = new List<string> { "Nhân viên bán hàng", "Nhân viên thủ kho" };
                List<string> GioiTinh = new List<string> { "Nam", "Nữ" };
                comboCV.DataSource = ChucVu;
                comboGT.DataSource = GioiTinh;
                //databinding
                txtMaNV.DataBindings.Clear();
                txtName.DataBindings.Clear();
                comboCV.DataBindings.Clear();
                comboGT.DataBindings.Clear();
                date.DataBindings.Clear();
                txtDiaChi.DataBindings.Clear();
                txtNumberPhone.DataBindings.Clear();
                //hiển thị dữ liệu vào các ô texdtBox, comboBox
                txtMaNV.DataBindings.Add("Text", dt, "MaNV");
                txtName.DataBindings.Add("Text", dt, "TenNV");
                comboCV.DataBindings.Add("Text", dt, "ChucVu");
                comboGT.DataBindings.Add("Text", dt, "GioiTinh");
                date.DataBindings.Add("Text", dt, "NgaySinh");
                txtDiaChi.DataBindings.Add("Text", dt, "DiaChi");
                txtNumberPhone.DataBindings.Add("Text", dt, "SDT");
            }
            catch
            {
                MessageBox.Show("Lỗi load dữ liệu từ database", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string TaoMaNV()
        {
            string ma = "";
            string taoma = "select MaNV from DSNhanVien where MaNV=(select max(MaNV) from DSNhanVien)";
            DataTable dt = kn.TaoBang(taoma);
            //nếu trong bảng chưa có mã thì tạo mới
            if (dt.Rows.Count <= 0)
            {
                ma = ma + Convert.ToString("NV001");
            }
            else//Nếu đã có mã trong bảng thì tăng mã hàng lên
            {
                //Lấy 3 ký tự cuối của chuyển thành int
                int k = Convert.ToInt32(dt.Rows[0]["MaNV"].ToString().Substring(dt.Rows[0]["MaNV"].ToString().Length - 3, 3));
                //sau đó tăng thêm 1 đơn vị
                k = k + 1;
                if (k < 10)
                {
                    ma = ma + "NV00" + k.ToString();
                }
                else if (k < 100)
                {
                    ma = ma + "NV0" + k.ToString();
                }
            }
            return ma;
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát không ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) this.Close();
        }

        private void frmQLNV_Load(object sender, EventArgs e)
        {
            Load_dulieu();
            //các nút button Them, Sua, Xoa, Luu bị ẩn => chọn làm mới nút Them sẽ hiển thị
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            btnSave.Enabled = false;
            btnRefresh.Focus();
            //các ô bị ẩn khi chưa nhấn làm mới
            txtMaNV.Enabled = false;
            txtName.Enabled = false;
            comboCV.Enabled = false;
            comboGT.Enabled = false;
            date.Enabled = false;
            txtDiaChi.Enabled = false;
            txtNumberPhone.Enabled = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataNV.Enabled = false;
            //các nút Sua, Xoa bị ẩn
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            //hiển thị các ô để nhập

            txtName.Enabled = true;
            comboCV.Enabled = true;
            comboGT.Enabled = true;
            date.Enabled = true;
            txtDiaChi.Enabled = true;
            txtNumberPhone.Enabled = true;
            //xóa tất cả dữ liệu có sẵn trong textBox
            txtMaNV.Clear();
            txtName.Clear();
            List<string> ChucVu = new List<string> { "Nhân viên bán hàng", "Nhân viên thủ kho" };
            List<string> GioiTinh = new List<string> { "Nam", "Nữ" };
            comboCV.DataSource = ChucVu;
            comboGT.DataSource = GioiTinh;
            date.Value = DateTime.Today;
            txtDiaChi.Clear();
            txtNumberPhone.Clear();
            //tạo mã nhân viên tự động
            txtMaNV.Text = TaoMaNV();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "" || txtName.Text == "")
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string them = "insert into DSNhanVien values('" + txtMaNV.Text + "',N'" + txtName.Text + "', N'" + comboCV.SelectedItem + "', N'" + comboGT.SelectedItem + "', '" + date.Value.ToString("yyyy/MM/dd") + "',N'" + txtDiaChi.Text + "', '" + txtNumberPhone.Text + "')";
                kn.TruyVan(them);
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_dulieu();
                dataNV.Enabled = true;
            }
        }

        private void dataNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //khi chọn hàng dataGridview các button bị ẩn
            btnAdd.Enabled = false;
            //các button Sua, Xoa hiện lên
            btnDelete.Enabled = true;
            btnPrepair.Enabled = true;
            //các ô textbox bị ẩn
            txtMaNV.Enabled = false;
            txtName.Enabled = false;
            comboCV.Enabled = false;
            comboGT.Enabled = false;
            date.Enabled = false;
            txtDiaChi.Enabled = false;
            txtNumberPhone.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnRefresh.Focus();
            string xoa = "delete from DSNhanVien where MaNV=N'" + txtMaNV.Text + "'";
            DialogResult kq = MessageBox.Show("Xóa nhân viên này?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                kn.TruyVan(xoa);
                MessageBox.Show("Xóa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Load_dulieu();
        }

        private void btnPrepair_Click(object sender, EventArgs e)
        {
            //Các nút button Xóa,sửa,làm mới bị ẩn
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            btnRefresh.Enabled = false;
            //mã nhân viên ẩn đi
            txtMaNV.Enabled = false;
            //các ô textBox hiện lên để sửa
            txtName.Enabled = true;
            comboCV.Enabled = true;
            comboGT.Enabled = true;
            date.Enabled = true;
            txtDiaChi.Enabled = true;
            txtNumberPhone.Enabled = true;
            //nút button lưu hiện lên để lưu lại
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string Save = "update DSNhanVien set MaNV=N'" + txtMaNV.Text + "', TenNV=N'" + txtName.Text + "', ChucVu=N'" + comboCV.SelectedValue.ToString() + "', GioiTinh=N'" + comboGT.SelectedValue.ToString() + "', NgaySinh=N'" + date.Value.ToString("yyyy/MM/dd") + "', DiaChi=N'" + txtDiaChi.Text + "', SDT=N'" + txtNumberPhone.Text + "' where MaNV='" + txtMaNV.Text + "'";
            kn.TruyVan(Save);
            MessageBox.Show("Sửa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Load_dulieu();
            //lưu dữ liệu xong, nút button Luu ẩn đi
            btnPrepair.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
            btnRefresh.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string timkiem = "select * from DSNhanVien where MaNV=N'" + txtSearchNV.Text.Trim() + "' or TenNV LIKE '%"+ txtSearchNV.Text.Trim() +"%'";
            DataTable dt = kn.TaoBang(timkiem);
            if (dt.Rows.Count > 0)
            {
                dataNV.DataSource = dt;
                List<string> ChucVu = new List<string> { "Nhân viên bán hàng", "Nhân viên thủ kho" };
                List<string> GioiTinh = new List<string> { "Nam", "Nữ" };
                comboCV.DataSource = ChucVu;
                comboGT.DataSource = GioiTinh;
                //databinding
                txtMaNV.DataBindings.Clear();
                txtName.DataBindings.Clear();
                comboCV.DataBindings.Clear();
                comboGT.DataBindings.Clear();
                date.DataBindings.Clear();
                txtDiaChi.DataBindings.Clear();
                txtNumberPhone.DataBindings.Clear();
                //hiển thị dữ liệu vào các ô texdtBox, comboBox
                txtMaNV.DataBindings.Add("Text", dt, "MaNV");
                txtName.DataBindings.Add("Text", dt, "TenNV");
                comboCV.DataBindings.Add("Text", dt, "ChucVu");
                comboGT.DataBindings.Add("Text", dt, "GioiTinh");
                date.DataBindings.Add("Text", dt, "NgaySinh");
                txtDiaChi.DataBindings.Add("Text", dt, "DiaChi");
                txtNumberPhone.DataBindings.Add("Text", dt, "SDT");
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearchNV.Clear();
            }
        }

        private void txtSearchNV_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtSearchNV_Click(object sender, EventArgs e)
        {
            Load_dulieu();
            txtSearchNV.Clear();
        }
    }
}
