using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris_02;
using WMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;


namespace Tetriss_02
{
    public partial class GameDisplay : Form
    {
        string sConn = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\EMBEDDED\\Documents\\dddd.mdf;Integrated Security = True; Connect Timeout = 30";
        Tetriss game;
        int sx; //스크린 가로
        int sy; //스크린 세로
        int bx; //보드 폭
        int by; //보드 높이
        int bwidth; //네모 한 칸의 x좌표
        int bheight; //네모 한 칸의 y좌표

        private Panel NextBlockPanel;
        string bgmFile = "C:\\Users\\EMBEDDED\\Desktop\\202401171122_stitch.mp3";

        WindowsMediaPlayer P = new WindowsMediaPlayer();
        
        public GameDisplay()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //폼 로드할 때 마다
        {
            game = Tetriss.tetirss;
            sx = GameScreen.S_Width; sy = GameScreen.S_Height;
            bx = GameScreen.BX;
            by = GameScreen.BY;
            bwidth = GameScreen.B_Width;
            bheight = GameScreen.B_Height;
            scLabel1.Location = new Point(395, 500);
            Levellabel.Location = new Point(395, 550);
            label2.Location = new Point(380, 25);
            
            SetClientSizeCore(500, 600); //500 * 600
            //P.URL = "C:\\Users\\EMBEDDED\\Desktop\\202401171122_stitch.mp3";
            //P.controls.stop();

            this.ClientSize = new Size(Size.Width+75, Size.Height-10);
            BGM();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DoubleBuffered = true;
            DrawGraduation(e.Graphics);
            DrawBrick(e.Graphics);
            DrawBoard(e.Graphics);
            DrawNextBrick(e.Graphics);
            DrawShadowBrick(e.Graphics);
        }
        
        private void DrawBoard(Graphics graphics)
        {
            int bn = game.BlockNum; //도형 종류(번호)
            for (int xx = 0; xx < bx; xx++)
            {
                for (int yy = 0; yy < by; yy++)
                {
                    if (game[xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle(xx * bwidth + 2,
                            yy * bheight + 2, bwidth - 3, bheight - 3);
                        graphics.FillRectangle(new SolidBrush(BlockValue.GetBlockColor(BlockValue.blockColorsArr[bn])), now_rt);
                    }
                }
            }
        }
        private void BGM()
        {
            P.URL = "C:\\Users\\EMBEDDED\\Desktop\\202401171122_stitch.mp3";
            P.controls.play();
            P.settings.setMode("loop", true);
        }
        public void BGB_Resume()
        {
            P.controls.play();
            P.settings.setMode("loop", true);
        }
        private void DrawNextBrick(Graphics graphics) //벽돌 생성
        {
            Pen dpen = new Pen(Color.Black, 1);
            int bn = game.BlockArr[1]; //도형 종류(번호)
            int tn = game.Turn;
            Console.WriteLine($"다음 블럭 : {game.BlockArr[1]}");
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, 0, xx, yy] != 0) //
                    {
                        Rectangle now_rt = new Rectangle((xx) * bwidth + 2 * 210, (yy)
                            * bheight + 2 * 30, bwidth - 3, bheight - 3);
                        graphics.DrawRectangle(dpen, now_rt);
                        graphics.FillRectangle(new SolidBrush(BlockValue.GetBlockColor(BlockValue.blockColorsArr[bn])), now_rt);
                    }
                }
            }
        }
        private void DrawShadowBrick(Graphics graphics) //벽돌 아래 미리보기
        {
            int alpha = 100; // 원하는 투명도 값을 설정 (0: 완전 투명, 255: 완전 불투명)

            Pen dpen = new Pen(Color.Gray, 1);
            Point brick = game.BrickPoint;
            int bn = game.BlockArr[0];
            int tn = game.Turn;

            // 현재 블록이 얼마나 아래로 이동할 수 있는지 계산
            int moveDownCount = 0;

            while (game.IsValidMove(0, moveDownCount + 1))
            {
                moveDownCount++;
            }

            // 알파 값을 적용한 블록 색상 생성
            Color blockColorWithAlpha = Color.FromArgb(alpha, BlockValue.GetBlockColor(BlockValue.blockColorsArr[bn]));

            // 이동 가능한 만큼 블록을 아래에 표시
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle shadowRect = new Rectangle((brick.X + xx) * bwidth + 2,
                            (brick.Y + yy + moveDownCount) * bheight + 2, bwidth - 3, bheight - 3);

                        graphics.DrawRectangle(dpen, shadowRect);
                        graphics.FillRectangle(new SolidBrush(blockColorWithAlpha), shadowRect);
                    }
                }
            }
        }
        private void DrawBrick(Graphics graphics) //벽돌 생성
        {
            Pen dpen = new Pen(Color.Black, 3);
            Point brick = game.BrickPoint;
            int bn = game.BlockArr[0]; //도형 종류(번호)
            int tn = game.Turn;
            for(int xx = 0; xx < 4; xx++)
            {
                for(int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0) //
                    {
                        Rectangle now_rt = new Rectangle((brick.X + xx) * bwidth + 2, (brick.Y + yy)
                            * bheight + 2, bwidth - 3, bheight - 3);
                        graphics.FillRectangle(new SolidBrush(BlockValue.GetBlockColor(BlockValue.blockColorsArr[bn])), now_rt);
                    }
                }
            }
        }
        private void DrawGraduation(Graphics graphics) //격자 그리기
        {
            Rectangle borderRect = new Rectangle(0, 0, bx * bwidth, by * bheight);
            graphics.DrawRectangle(Pens.White, borderRect);
           //DrawHorizons(graphics);
           DrawVerticals(graphics);
        }
        private void DrawVerticals(Graphics graphics) //격자 - 수직
        {
            Point st = new Point(360,335);
            Point et = new Point(600,335);
            Pen pen = new Pen(Color.White, 1);
            
            graphics.DrawLine(pen, st, et);
        }
        private void DrawHorizons(Graphics graphics) //격자 - 수평
        {
            Point st = new Point();
            Point et = new Point();
            for (int cy = 0; cy < by; cy++) //맵의 아래쪽 끝까지
            {
                st.X = 0;
                st.Y = cy * bheight; //네모 한 칸의 세로 길이
                et.X = bx * bwidth;
                et.Y = st.Y;
                graphics.DrawLine(Pens.Black, st, et);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode) 
            {
                case Keys.Right: MoveRight(); return;
                case Keys.Left: MoveLeft(); return;
                case Keys.Down: MoveDown(); return;
                case Keys.Space: SpaceMoveDown(); return;
                case Keys.Up: MoveTurn(); return;
                case Keys.Escape: pause(); return;
            }
        }
        private void MoveRight()
        {
            if (game.MoveRight())
            {
                Region rg = MakeRegion(-1, 0);
                Invalidate(rg);
            }  
        }
        private void MoveLeft()
        {
            if(game.MoveLeft())
            {
                Region rg = MakeRegion(1, 0);
                Invalidate(rg);
            }
        }
        private void MoveDown()
        {
            if(game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
                
            }
            else
            {
                EndingCheck();
            }
        }
        private void SpaceMoveDown()
        {
            while (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            EndingCheck();
        }
        private void EndingCheck() //게임 끝내기 여부
        {
            if(game.Next())
            {
                Invalidate();           
            }
            else
            {
                timer1.Enabled = false;
                RankStore();
            }
        }
        private void RankStore()
        {
            if (DialogResult.Yes == MessageBox.Show("저장하겠습니까?",
                    "저장 여부", MessageBoxButtons.YesNo))
            {
                using (GetUserName getNameForm = new GetUserName())
                {
                    if (getNameForm.ShowDialog() == DialogResult.OK)
                    {
                        string userName = getNameForm.PlayerName;
                        int userrank = getNameForm.PlayerRank;
                        using (SqlConnection sqlConnection = new SqlConnection(sConn))
                        {
                            sqlConnection.Open();

                            // SQL 쿼리 작성
                            string sql = $"INSERT INTO DBBrank (Name, Score) VALUES (N'{userName}', {Board.score})";
                            // SQL 쿼리 실행
                            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }
                        }

                    }
                }
                ExitKeepMenu total = new ExitKeepMenu();
                total.Show();
                timer1.Enabled = true;
                game.ReStart();
                Close();
                Invalidate();
            }
            else
            {
                ExitKeepMenu total = new ExitKeepMenu();
                total.Show();
                timer1.Enabled = true;
                game.ReStart();
                Close();
                Invalidate();
            }
        }
        private void MoveTurn()
        {
            if (game.MoveTurn())
            {
                Region rg = MakeRegion();
                Invalidate(rg);
            }
        }

        private Region MakeRegion(int cx, int cy) //기존 사각형, 움직인 사각형 생성 후 rg1에 업데이트 
        {
            Point now = game.BrickPoint;
            int bn = game.BlockNum;
            int tn = game.Turn;
            Region region = new Region();
            for(int xx = 0; xx < 4; xx++)
            {
                for(int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2
                            , bwidth - 3, bheight - 3);
                        Rectangle rect2 = new Rectangle((now.X + cx + xx) * bwidth
                            , (now.Y + cy + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        Region rg2 = new Region(rect2);
                        region.Union(rg1);
                        region.Union(rg2);
                        scLabel1.Text = $"Score : {Board.score}"; //점수 라벨1에 나타냄
                        Levellabel.Text = $"Level : {Board.Level}"; //레벨을 라벨에 나타냄
                        flashScreen();
                    }
                }
            }
            return region;
        }
        private Region MakeRegion()
        {
            Point now = game.BrickPoint;
            int bn = game.BlockNum;
            int tn = game.Turn;
            int oldtn = (tn + 3) % 4;
            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2
                            , bwidth - 3, bheight - 3);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                    if (BlockValue.bvals[bn, oldtn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth
                            , (now.Y + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                }
            }
            return region;
        }

        private void timer1_Tick(object sender, EventArgs e) //테트리스 내려오는 시간(기본)
        {
            MoveDown();
            //레벨에 따른 시간 제어
            if (Board.Level == 1) timer1.Interval = 1000;
            else if (Board.Level == 2) timer1.Interval = 800;
            else if (Board.Level == 3) timer1.Interval = 600;
            else if (Board.Level == 4) timer1.Interval = 400;
            else if (Board.Level == 5) timer1.Interval = 200;
            OnTimerElapsed();
        }

        private void pause()
        {
            timer1.Enabled = false;
            GamePause pause = new GamePause(this); // 메뉴 폼 인스턴스 생성
            pause.Show();
            P.controls.pause();
            
        }
        public void OnTimerElapsed()
        {
            timer1.Enabled = true;
            Invalidate();
        }

        int currentLevel = 0;
        private void flashScreen()
        {
            if (currentLevel != Board.Level)
            {
                currentLevel = Board.Level;
                FlashScreen(Color.White, 1);
            }
        }
        private async void FlashScreen(Color flashColor, int numFlashes)
        {
            for (int i = 0; i < numFlashes; i++)
            {
                BackColor = flashColor;
                Refresh();
                await Task.Delay(100);
                BackColor = SystemColors.Control;
                Refresh();
                await Task.Delay(100);
            }
            BackColor = Color.Black;
        }
    }
}
