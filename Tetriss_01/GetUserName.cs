using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Tetriss_02
{
    public partial class GetUserName : Form
    {
        public GetUserName()
        {
            InitializeComponent();
            label1.Parent = pictureBox1;
        }
        public string PlayerName { get; private set; }
        public int PlayerRank = 1;

        private void Namebutton_Click(object sender, EventArgs e)
        {
            PlayerName = UserTextBox.Text;
            PlayerRank++;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
