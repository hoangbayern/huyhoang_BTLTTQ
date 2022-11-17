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
    public partial class frmHDNH : Form
    {
        KetNoiDuLieu kn = new KetNoiDuLieu();
        public frmHDNH()
        {
            InitializeComponent();
        }

        private void Load_dulieu()
        {
            try
            {
                string sql = "select * from TaoPhieuNhap";
                DataTable dt = kn.TaoBang(sql);
                dataHDN.DataSource = dt;
                //databindings
                txtMaPN.DataBindings.Clear();
                comboMaNCC.DataBindings.Clear();
                comboMaKho.DataBindings.Clear();
                comboMaNV.DataBindings.Clear();
                txtTenNguoitao.DataBindings.Clear();
                txtGhiChu.DataBindings.Clear();

                txtMaPN.DataBindings.Add("Text", dt, "MaPN");
                comboMaNCC.DataBindings.Add("Text", dt, "MaNCC");
                comboMaKho.DataBindings.Add("Text", dt, "MaKho");
                comboMaNV.DataBindings.Add("Text", dt, "MaNV");
                txtTenNguoitao.DataBindings.Add("Text", dt, "TenNguoiTao");
                txtGhiChu.DataBindings.Add("Text", dt, "GhiChu");
            }
            catch
            {
                MessageBox.Show("Lỗi load dữ liệu từ bảng TaoPhieuNhap", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string TaoMaPN()
        {
            string ma = "";
            string taoma = "select MaPN from TaoPhieuNhap where MaPN=(select max(MaPN) from TaoPhieuNhap)";
            DataTable dt = kn.TaoBang(taoma);
            //nếu trong bảng chưa có mã thì tạo mới
            if (dt.Rows.Count <= 0)
            {
                ma = ma + Convert.ToString("PN0001");
            }
            else//Nếu đã có mã trong bảng thì tăng mã hàng lên
            {
                //Lấy 3 ký tự cuối của chuyển thành int
                int k = Convert.ToInt32(dt.Rows[0]["MaPN"].ToString().Substring(dt.Rows[0]["MaPN"].ToString().Length - 3, 3));
                //sau đó tăng thêm 1 đơn vị
                k = k + 1;
                if (k < 10)
                {
                    ma = ma + "PN000" + k.ToString();
                }
                else if (k < 100)
                {
                    ma = ma + "PN00" + k.ToString();
                }
                else if (k < 1000)
                {
                    ma = ma + "PN0" + k.ToString();
                }
            }
            return ma;
        }

        private void Load_MaNCC()
        {
            try
            {
                string mancc = "select MaNCC from DSNhaCungCap";
                DataTable dt = kn.TaoBang(mancc);
                comboMaNCC.DataSource = dt;
                comboMaNCC.DisplayMember = "MaNCC";
                comboMaNCC.ValueMember = "MaNCC";
            }
            catch
            {
                MessageBox.Show("Lỗi load dữ liệu từ bảng Danh sách NCC", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Load_MaKho()
        {
            try
            {
                string makho = "select MaKho from DSKho";
                DataTable dt = kn.TaoBang(makho);
                comboMaKho.DataSource = dt;
                comboMaKho.ValueMember = "MaKho";
            }
            catch
            {
                MessageBox.Show("Lỗi load dữ liệu từ bảng DSMaKho", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Load_MaNV_TenNguoiTao()
        {
            try
            {
                string manv = "select MaNV from DSNhanVien";
                DataTable dt = kn.TaoBang(manv);
                if (dt.Rows.Count > 0)
                {
                    comboMaNV.DataSource = dt;
                    comboMaNV.ValueMember = "MaNV";
                }
            }
            catch
            {
                MessageBox.Show("Lỗi load dữ liệu từ bảng DSNhanVien", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmHDNH_Load(object sender, EventArgs e)
        {
            Load_dulieu();
            Load_MaNCC();
            Load_MaKho();
            Load_MaNV_TenNguoiTao();
            //các nút button Them, Sua, Xoa, Luu bị ẩn => chọn làm mới nút Them sẽ hiển thị
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            btnSave.Enabled = false;
            btnRefresh.Focus();
            //các ô bị ẩn khi chưa nhấn làm mới
            txtMaPN.Enabled = false;
            comboMaNCC.Enabled = false;
            comboMaKho.Enabled = false;
            comboMaNV.Enabled = false;
            txtTenNguoitao.Enabled = false;
            txtGhiChu.Enabled = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataHDN.Enabled = false;
            btnRefresh.Focus();
            //các nút Sua, Xoa bị ẩn
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            //hiển thị các ô để nhập
            comboMaNCC.Enabled = true;
            comboMaKho.Enabled = true;
            comboMaNV.Enabled = true;
            txtTenNguoitao.Enabled = true;
            txtGhiChu.Enabled = true;
            //xóa tất cả dữ liệu có sẵn trong textBox
            txtMaPN.Clear();
            comboMaNCC.SelectedItem = null;
            comboMaKho.SelectedItem = null;
            comboMaNV.SelectedItem = null;
            txtTenNguoitao.Clear();
            txtGhiChu.Clear();
            ///tạo mã PN tự động
            txtMaPN.Text = TaoMaPN();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtMaPN.Text == "" || comboMaNCC.SelectedValue == null || comboMaKho.SelectedValue == null || comboMaNV.SelectedValue == null || txtTenNguoitao.Text == "")
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataHDN.Enabled = true;
            }
            else
            {
                string them = "insert into TaoPhieuNhap values('" + txtMaPN.Text + "','" + comboMaNCC.SelectedValue + "','" + comboMaKho.SelectedValue + "','" + comboMaNV.SelectedValue + "',N'" + txtTenNguoitao.Text + "',N'" + txtGhiChu.Text + "')";
                kn.TruyVan(them);
                MessageBox.Show("Thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_dulieu();
                dataHDN.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string xoa = "delete from TaoPhieuNhap where MaPN='" + txtMaPN.Text + "'";
            DialogResult kq = MessageBox.Show("Bạn có muốn xóa phiếu nhập này?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                kn.TruyVan(xoa);
                Load_dulieu();
                MessageBox.Show("Xóa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrepair_Click(object sender, EventArgs e)
        {
            //nút button Xoa bị ẩn
            btnDelete.Enabled = false;
            btnRefresh.Enabled = false;
            //mã phiếu nhập ẩn 
            txtMaPN.Enabled = false;
            //các ô textBox hiện lên để sửa
            comboMaNCC.Enabled = true;
            comboMaKho.Enabled = true;
            comboMaNV.Enabled = true;
            txtTenNguoitao.Enabled = true;
            txtGhiChu.Enabled = true;
            //nút button lưu hiện lên để lưu lại
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string Save = "update TaoPhieuNhap set MaPN=N'" + txtMaPN.Text + "', MaNCC=N'" + comboMaNCC.SelectedValue + "', MaKho=N'" + comboMaKho.SelectedValue + "', MaNV=N'" + comboMaNV.SelectedValue.ToString() + "', TenNguoiTao=N'" + txtTenNguoitao.Text + "' , GhiChu=N'" + txtGhiChu.Text + "'  where MaPN=N'" + txtMaPN.Text + "'";
            kn.TruyVan(Save);
            Load_dulieu();
            MessageBox.Show("Sửa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //lưu dữ liệu xong, nút button Luu ẩn đi
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
            btnRefresh.Enabled = true;
        }

        private void dataHDN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //khi chọn hàng dataGridview các button bị ẩn
            btnAdd.Enabled = false;
            //các button Sua, Xoa hiện lên
            btnDelete.Enabled = true;
            btnPrepair.Enabled = true;
            //các ô textbox bị ẩn
            txtMaPN.Enabled = false;
            comboMaNCC.Enabled = false;
            comboMaKho.Enabled = false;
            comboMaNV.Enabled = false;
            txtTenNguoitao.Enabled = false;
            txtGhiChu.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập nội dung tìm kiếm!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string timkiem = "select * from TaoPhieuNhap where MaPN='" + txtSearch.Text + "' or MaNCC='" + txtSearch.Text + "' or MaKho='" + txtSearch.Text + "' or MaNV='" + txtSearch.Text + "' or TenNguoiTao=N'" + txtSearch.Text + "' or TenNguoiTao like N'%" + txtSearch.Text + "%'";
            DataTable dt = kn.TaoBang(timkiem);
            if (dt.Rows.Count > 0)
            {
                dataHDN.DataSource = dt;
                //databindings
                txtMaPN.DataBindings.Clear();
                comboMaNCC.DataBindings.Clear();
                comboMaKho.DataBindings.Clear();
                comboMaNV.DataBindings.Clear();
                txtTenNguoitao.DataBindings.Clear();
                txtGhiChu.DataBindings.Clear();

                txtMaPN.DataBindings.Add("Text", dt, "MaPN");
                comboMaNCC.DataBindings.Add("Text", dt, "MaNCC");
                comboMaKho.DataBindings.Add("Text", dt, "MaKho");
                comboMaNV.DataBindings.Add("Text", dt, "MaNV");
                txtTenNguoitao.DataBindings.Add("Text", dt, "TenNguoiTao");
                txtGhiChu.DataBindings.Add("Text", dt, "GhiChu");
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearch.Clear();
                Load_dulieu();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboMaNV_TextChanged(object sender, EventArgs e)
        {
            string manv = "select TenNV from DSNhanVien where MaNV=N'" + comboMaNV.SelectedValue + "'";
            DataTable dt = kn.TaoBang(manv);
            if (dt.Rows.Count > 0)
            {
                txtTenNguoitao.Text = dt.Rows[0]["TenNV"].ToString();
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            Load_dulieu();
            txtSearch.Clear();
        }
    }
}
