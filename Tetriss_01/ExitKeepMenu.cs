using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetriss_01;

namespace Tetriss_02
{
    public partial class ExitKeepMenu : Form
    {
        public ExitKeepMenu()
        {
            InitializeComponent();
            label1.Parent = this.pictureBox230;
            label2.Parent = this.pictureBox230;
            label3.Parent = this.pictureBox230;
        }

        private void KeepGo_Click(object sender, EventArgs e)
        {
            GameDisplay game = new GameDisplay();    //싱글 테트리스 게임 인스턴스 생성
            game.Show();
            //timer1.Enabled = true;
            Invalidate();
            Close();
        }

        private void GOMenu_1_Click(object sender, EventArgs e)
        {
            Loding menu = new Loding(); // 메뉴 폼 인스턴스 생성
            menu.Show();
            Close();
        }

        private void Exit_1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
