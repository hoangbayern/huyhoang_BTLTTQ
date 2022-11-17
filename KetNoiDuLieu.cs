using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finnalBTL
{
    internal class KetNoiDuLieu
    {
        /*Thay chuỗi kết nối trong ngoặc tùy vào máy*/
        public SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QR4E84J\SQLEXPRESS;Initial Catalog=QuanLyShopTC;Integrated Security=True");
        public void MoKetNoi()
        {
            con.Open();
        }
        public void DongKetNoi()
        {
            con.Close();
        }
        //trả về một database
        public DataTable TaoBang(string sql)
        {
            //MoKetNoi();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
            return dt;
        }
        //thực thi câu lệnh truy vấn insert, update, delete
        public void TruyVan(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            MoKetNoi();
            cmd.ExecuteNonQuery();
            DongKetNoi();
        }
    }
}
