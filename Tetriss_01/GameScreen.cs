using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetriss_01;
using Tetriss_02;

namespace Tetris_02
{
    internal static class GameScreen
    {
        //스크린 넓이
        internal const int S_Width = 500;
        internal const int S_Height = 600;
        //한 칸의 넓이
        internal const int B_Width = 30;
        internal const int B_Height = 30;
        //칸의 갯수
        internal const int BX = 12;
        internal const int BY = 20;
        //도형의 시작 점
        internal const int SX = 4;
        internal const int SY = 0;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Loding());
        }

    }
}
