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
    public partial class frmQLSP : Form
    {
        KetNoiDuLieu kn = new KetNoiDuLieu();
        public frmQLSP()
        {
            InitializeComponent();
        }

        private void Load_dulieu()
        {
            try
            {
                string sql = "select * from DSHangHoa";
                DataTable dt = kn.TaoBang(sql);
                dataSP.DataSource = dt;
                //databinding
                txtMaHang.DataBindings.Clear();
                txtTenHang.DataBindings.Clear();
                txtGia.DataBindings.Clear();
                txtSL.DataBindings.Clear();
                txtXuatXu.DataBindings.Clear();
                dateHSD.DataBindings.Clear();
                //hiển thị dữ liệu vào các ô texdtBox, comboBox
                txtMaHang.DataBindings.Add("Text", dt, "MaH");
                txtTenHang.DataBindings.Add("Text", dt, "TenH");
                txtGia.DataBindings.Add("Text", dt, "DonGia");
                txtSL.DataBindings.Add("Text", dt, "SoLuong");
                txtXuatXu.DataBindings.Add("Text", dt, "XuatXu");
                dateHSD.DataBindings.Add("Text", dt, "HanSD");
            }
            catch
            {
                MessageBox.Show("Lỗi load dữ liệu từ database", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string TaoMaHang()
        {
            string ma = "";
            string taoma = "select MaH from DSHangHoa where MaH=(select max(MaH) from DSHangHoa)";
            DataTable dt = kn.TaoBang(taoma);
            //nếu trong bảng DSHangHoa chưa có mã hàng thì tạo mới
            if (dt.Rows.Count <= 0)
            {
                ma = ma + Convert.ToString("MH001");
            }
            else//Nếu đã có mã hàng trong bảng thì tăng mã hàng lên
            {
                //Lấy 3 ký tự cuối của MaH chuyển thành int
                int k = Convert.ToInt32(dt.Rows[0]["MaH"].ToString().Substring(dt.Rows[0]["MaH"].ToString().Length - 3, 3));
                //sau đó tăng thêm 1 đơn vị
                k = k + 1;
                if (k < 10)
                {
                    ma = ma + "MH00" + k.ToString();
                }
                else if (k < 100)
                {
                    ma = ma + "MH0" + k.ToString();
                }
            }
            return ma;
        }

        private void frmQLSP_Load(object sender, EventArgs e)
        {
            Load_dulieu();
            //các nút button Them, Sua, Xoa, Luu bị ẩn => chọn Refresh nút Thêm sẽ hiển thị
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            btnSave.Enabled = false;
            btnRefresh.Focus();
            //các ô bị ẩn khi chưa nhấn làm mới
            txtMaHang.Enabled = false;
            txtTenHang.Enabled = false;
            txtGia.Enabled = false;
            txtSL.Enabled = false;
            txtXuatXu.Enabled = false;
            dateHSD.Enabled = false;
        }

        private void dataSP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataSP.ColumnHeadersHeight = 28;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataSP.Enabled = false;
            //các nút Sua, Xoa bị ẩn
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnPrepair.Enabled = false;
            //hiển thị các ô để nhập
            txtTenHang.Enabled = true;
            txtGia.Enabled = true;
            txtXuatXu.Enabled = true;
            txtSL.Enabled = true;
            dateHSD.Enabled = true;
            //xóa tất cả dữ liệu có sẵn trong textBox
            txtMaHang.Clear();
            txtTenHang.Clear();
            txtGia.Clear();
            txtSL.Clear();
            txtXuatXu.Clear();
            dateHSD.Value = DateTime.Now;
            //Tạo mã hàng mới
            txtMaHang.Text = TaoMaHang();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtMaHang.Text == "" || txtTenHang.Text == "" || txtGia.Text == "" || txtSL.Text == "" || txtXuatXu.Text == "")
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string them = "insert into DSHangHoa values(N'" + txtMaHang.Text + "',N'" + txtTenHang.Text + "', N'" + txtGia.Text + "', '" + txtSL.Text + "',N'" + txtXuatXu.Text + "','" + dateHSD.Value.ToString("yyyy/MM/dd") + "')";
                kn.TruyVan(them);
                MessageBox.Show("Thêm thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Load_dulieu();
                dataSP.Enabled = true;
            }
        }

        private void dataSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //khi chọn hàng dataGridview các button bị ẩn
            btnAdd.Enabled = false;
            //các button Sua, Xoa hiện lên
            btnDelete.Enabled = true;
            btnPrepair.Enabled = true;
            //các ô textbox bị ẩn
            txtTenHang.Enabled = false;
            txtGia.Enabled = false;
            txtSL.Enabled = false;
            txtXuatXu.Enabled = false;
            dateHSD.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string xoa = "delete from DSHangHoa where MaH='" + txtMaHang.Text + "'";
            DialogResult kq = MessageBox.Show("Xóa mặt hàng này?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            btnRefresh.Enabled = false;
            //mã sản phẩm ẩn đi
            txtMaHang.Enabled = false;
            //các ô textBox hiện lên để sửa
            txtTenHang.Enabled = true;
            txtGia.Enabled = true;
            txtSL.Enabled = true;
            txtXuatXu.Enabled = true;
            dateHSD.Enabled = true;
            //nút button lưu hiện lên để lưu lại
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string luu = "update DSHangHoa set MaH='" + txtMaHang.Text + "', TenH=N'" + txtTenHang.Text + "', DonGia='" + txtGia.Text + "', SoLuong='" + txtSL.Text + "', XuatXu=N'" + txtXuatXu.Text + "', HanSD='" + dateHSD.Value.ToString("yyyy/MM/dd") + "' where MaH='" + txtMaHang.Text + "'";
            kn.TruyVan(luu);
            MessageBox.Show("Sửa thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Load_dulieu();
            //lưu dữ liệu xong, nút button Luu ẩn đi
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
            btnRefresh.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string timkiem = "select * from DSHangHoa where MaH=N'" + txtSearchSP.Text.Trim() + "' or TenH like '%"+ txtSearchSP.Text.Trim() +"%'";
            DataTable dt = kn.TaoBang(timkiem);
            if (dt.Rows.Count > 0)
            {
                dataSP.DataSource = dt;
                //databinding
                txtMaHang.DataBindings.Clear();
                txtTenHang.DataBindings.Clear();
                txtGia.DataBindings.Clear();
                txtSL.DataBindings.Clear();
                txtXuatXu.DataBindings.Clear();
                dateHSD.DataBindings.Clear();
                //hiển thị dữ liệu vào các ô texdtBox
                txtMaHang.DataBindings.Add("Text", dt, "MaH");
                txtTenHang.DataBindings.Add("Text", dt, "TenH");
                txtGia.DataBindings.Add("Text", dt, "DonGia");
                txtSL.DataBindings.Add("Text", dt, "SoLuong");
                txtXuatXu.DataBindings.Add("Text", dt, "XuatXu");
                dateHSD.DataBindings.Add("Text", dt, "HanSD");
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearchSP.Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearchSP_Click(object sender, EventArgs e)
        {
            Load_dulieu();
            txtSearchSP.Clear();
        }
    }
}
