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
    public partial class frmDangNhap : Form
    {
        KetNoiDuLieu kn = new KetNoiDuLieu();
        public frmDangNhap()
        {
            InitializeComponent();
            this.Cursor = Cursors.Hand;
            txtUser.Focus();
        }

        private void btnLogin_MouseClick(object sender, MouseEventArgs e)
        {
            btnLogin.BackColor = Color.Aquamarine;

        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor= Color.Pink;
        }

       

        private void btnExit_MouseClick_1(object sender, MouseEventArgs e)
        {
            btnExit.BackColor = Color.Aquamarine;
        }

        private void btnExit_MouseLeave_1(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.Pink;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát không ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) this.Close();

        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                txtUser.Focus();
                this.errorProvider1.SetError(txtUser, "Bạn chưa nhập tên đăng nhập!");
            }
            else
            {
                this.errorProvider1.Clear();
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Focus();
                this.errorProvider1.SetError(txtPassword, "Bạn chưa nhập mật khẩu!");
            }
            else
            {
                this.errorProvider1.Clear();
            }
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            btnLogin.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên đăng nhập!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            try
            {

                string sql = "select * from DSTaiKhoan where TenDangNhap = '" + txtUser.Text + "'and MatKhau ='" + txtPassword.Text + "'";
                DataTable dt = kn.TaoBang(sql);
                //kiểm tra dữ liệu lấy được từ SQL => datatable
                //kiểm tra nếu có thì phân quyền đăng nhập
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    //duyệt tất cả hàng trong datatable, truyền dữ liệu cột Quyen vào FormTrangChu
                    //FormTrangChu sẽ khai báo 1 biến để gán dữ liệu vừa truyền vào
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        frmTrangChu.userName = txtUser.Text;
                        frmTrangChu f = new frmTrangChu();
                        f.Show();
                        this.Hide();
                        f.DangXuat += F_DangXuat;
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception loi)
            {
                MessageBox.Show("Lỗi kết nối tới bảng DSTaiKhoan", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(loi.Message);
            }
        }

        private void F_DangXuat(object sender, EventArgs e)
        {
            (sender as frmTrangChu).isThoat = false;
            (sender as frmTrangChu).Close();
            //form đăng nhập hiện lên
            this.Show();
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
