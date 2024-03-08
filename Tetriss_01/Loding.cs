using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetriss_02;
using System.Media;
using WMPLib;


namespace Tetriss_01
{
    public partial class Loding : Form
    {
        Tetriss game;
        WindowsMediaPlayer SP = new WindowsMediaPlayer();

        public Loding()
        {
            InitializeComponent();
            label1.Parent = pictureBox1;
            label2.Parent = pictureBox1;
            label3.Parent = pictureBox1;
            bgm_type();
        }
        private void bgm_type()
        {
            SP.URL = "C:\\Users\\EMBEDDED\\Desktop\\initialmusic.m4a";
            SP.controls.play();
            SP.settings.setMode("loop", true);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            game = Tetriss.tetirss;
            GameDisplay game1 = new GameDisplay();    //싱글 테트리스 게임 인스턴스 생성
            game1.Show();                             //게임을 위한 창띄우기
            game.ReStart();
            this.Hide();
            SP.controls.stop();
        }

        private void RankButton_Click(object sender, EventArgs e)
        {
            ViewRank rank = new ViewRank();    //싱글 테트리스 게임 인스턴스 생성
            rank.Show();                             //게임을 위한 창띄우기
            this.Hide();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
