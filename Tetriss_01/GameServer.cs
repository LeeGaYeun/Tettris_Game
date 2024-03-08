using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Tetriss_02
{
    class GameServer
    {
        internal static void gameServer()
        {
            string strConn = "Server=127.0.0.1;Port=3306;Database=GameDB;UserID=root;PWD=0000;";

            MySqlConnection conn = new MySqlConnection(strConn);

            try
            {
                conn.Open();
                if(conn.State == System.Data.ConnectionState.Open)
                {
                    MessageBox.Show("연결성공");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("연결실패");
            }

        }
    }
}
