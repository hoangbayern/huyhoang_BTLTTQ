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
    public partial class frmTrangChu : Form
    {
        public static string userName = "lỗi";
        public frmTrangChu()
        {
            InitializeComponent();
        }
        private Form currentFormChild;
        private void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_Body.Controls.Add(childForm);
            panel_Body.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void frmTrangChu_Load(object sender, EventArgs e)
        {
            lbuser.Text = "Hi , " + userName;
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void btnNV_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQLNV());
        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQLNCC());
        }

        private void btnSP_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQLSP());
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmHDNH());
        }

        private void btnXuatHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQLXH());
        }

        private void btnHDB_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmHDBH());
        }

        private void btnKH_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQLKH());
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQLTK());
        }


        //biến kiểm tra
        public bool isThoat = true;
        
        public event EventHandler DangXuat;
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn chắc chắn muốn đăng xuất", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                //ủy thác cho nơi khác chạy lệnh tại đó
                DangXuat(this, new EventArgs());
            }
        }
    }
}
