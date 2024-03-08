using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetriss_01;

namespace Tetriss_02
{
    public partial class ViewRank : Form
    {
        string sConn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\EMBEDDED\\Documents\\dddd.mdf;Integrated Security=True;Connect Timeout=30";

        public ViewRank()
        {
            InitializeComponent();
            LoadDataToDataGridView();
        }

        private void LoadDataToDataGridView()
        {
            using (SqlConnection sqlConnection = new SqlConnection(sConn))
            {
                sqlConnection.Open();

                // 순위를 계산하여 정렬된 데이터를 가져오는 SQL 쿼리
                string sql = "SELECT " +
                             "DENSE_RANK() OVER (ORDER BY Score DESC) AS Rank, " +
                             "Name, Score " +
                             "FROM DBBrank ORDER BY Score DESC;";

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlDataReader sr = sqlCommand.ExecuteReader();

                    // DataGridView 초기화
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    // Rank 컬럼 추가 (좌측에 표시)
                    dataGridView1.Columns.Add("Rank", "Rank");

                    // 나머지 컬럼 추가
                    for (int i = 1; i < sr.FieldCount; i++)
                    {
                        string colName = sr.GetName(i);
                        dataGridView1.Columns.Add(colName, colName);
                    }

                    // 데이터 읽어서 DataGridView에 추가
                    while (sr.Read())
                    {
                        int nRow = dataGridView1.Rows.Add();
                        for (int i = 0; i < sr.FieldCount; i++)
                        {
                            object o = sr.GetValue(i);
                            dataGridView1.Rows[nRow].Cells[i].Value = o;
                        }
                    }
                }
            }
        }

        private void GoMenu_Click(object sender, EventArgs e)
        {
            Loding menu = new Loding(); // 메뉴 폼 인스턴스 생성
            menu.Show();
            this.Close();
        }

        private void CloseWin_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}