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
    public partial class GamePause : Form
    {
        private GameDisplay gameDisplay;
        public GamePause(GameDisplay gameDisplay)
        {
            InitializeComponent();
            this.gameDisplay = gameDisplay;
        }

        private void MnuButton_Click(object sender, EventArgs e)
        {
            Loding menu = new Loding(); // 메뉴 폼 인스턴스 생성
            menu.Show();
            gameDisplay.Close();
            Close();
        }

        private void KeepButton_Click(object sender, EventArgs e)
        {
            gameDisplay.OnTimerElapsed();
            gameDisplay.BGB_Resume();
            Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
