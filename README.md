# Tettris_Game
## 1. 프로젝트

- 프로젝트 명 : 테트리스 게임
- 시나리오
    - 테트리스 게임 구현
        - 싱글 플레이
        - 점수에 따른 레벨 변화
        - 난이도 변화 속도 증가, 변형 도형 생성, 하단 블록 생성
        - 다음 도형 미리 보기
        - 도형이 놓일 예상 위치 표시
    - 게임의 난이도 구현
    - 데이터 베이스와 게임과의 연동 시스템 구현
        - 점수 저장
        - 점수에 따른 랭킹 확인
        - 최고 점수 게임 화면에 표시
    - 게임 서버 시스템 개발
        - 2인 이상 가능한 멀티 플레이 개발
        - 멀티 플레이 중 아이템 사용 및 화면 이펙트

## 2. 작업순서

[간트차트](https://www.notion.so/e9b6445b81e040b689bc188875a2b23c?pvs=21)

[칸반보드](https://www.notion.so/14e8db2025b44c82a16b71437acd92d9?pvs=21)

## 3. 다이어그램

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/de458be3-9f2d-41fc-b9d0-2f0b628fafc9/df0fdf77-64fe-41ac-bda3-66fdb4755062/Untitled.png)

## 4. 소스코드 분석

- 주요 코드 분석
    - **게임 보드의 공간, 좌표, 블럭이 떨어질 시작 좌표**
        
        
        ```csharp
         /* 좌상단 0,0 기준 360 * 600 size */
        
         //블럭 한 칸의 가로, 세로 길이
         internal const int B_Width = 30;
         internal const int B_Height = 30;
         //블럭 갯수
         internal const int BX = 12;
         internal const int BY = 20;
         //도형의 시작 점(테트리스 도형의 시작점)
         internal const int SX = 4;
         internal const int SY = 0;
        ```
        
    - **벽돌의 움직임**
        
        키보드 입력에 따른 블록의 이동 및 회전이다.
        
        이때, 도형의 겹침과 게임 보드를 벗어나는 지의 여부를 확인하여 이동 또는 회전이 가능한지의 여부를 나타낸다.
        
        ```csharp
        //벽돌의 움직임
        
        /* 
        키보드 좌/우/아래/위 화살표를 입력받아 함수 실행 
        보드의 x,y 좌표(xx, yy)와 도형의 x,y(brick.X, brick.Y) 좌표를 비교해
        쌓여있는 도형과의 겹침 방지 ,이동 시 게임 화면 밖으로 나가는거 방지 여부를 false , true 반환
        */
        
        //벽돌이 이동 가능한지 확인
        internal bool MoveLeft()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[brick.BlockArr[0], Turn, xx, yy] != 0) //쌓여있는 도형과의 겹침 방지
                    {
                        if (brick.X + xx <= 0) //회전 시 게임화면 왼쪽 밖으로 나가는거 방지
                        {
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X - 1, brick.Y))
            {
                brick.MoveLeft();
                return true;
            }
            return false;
        }
        internal bool MoveRight()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[brick.BlockArr[0], Turn, xx, yy] != 0)
                    {
                        if ((brick.X + xx + 1) >= GameScreen.BX) //회전 시 게임화면 오른쪽 밖으로 나가는거 방지
                        {
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X + 1, brick.Y))
            {
                brick.MoveRight();
                return true;
            }
            return false;
        }
        //1. timer 이벤트를 이용하여 일정 시간마다 MoveDown 메소드 호출
        //2. MoveDown 메소드 실행 시 gboard.MoveEnable 메소드를 호출
        //3. gboard.MoveEnable 메소드는 보드 x,y 좌표와 도형 x,y 좌표를 비교 -> true 반환시 MoveDown 반복
        //4. gboard.MoveEnable 메소드 false 반환 시 gboard.Store 함수 호출
        //5. gboard.Store 호출하여 해당 좌표 보드 영역에 도형의 데이터값 대입
        internal bool MoveDown()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[brick.BlockArr[0], Turn, xx, yy] != 0)
                    {
                        if ((brick.Y + yy + 1) >= GameScreen.BY)
                        {
                            gboard.Store(brick.BlockArr[0], Turn, brick.X, brick.Y);
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X, brick.Y + 1))
            {
                brick.MoveDown();
                return true;
            }
            gboard.Store(brick.BlockArr[0], Turn, brick.X, brick.Y);
            return false;
        }
        //1. timer 이벤트를 이용하여 일정 시간마다 MoveTurn 메소드 호출
        //2. MoveTurn 메소드 실행 시 gboard.MoveEnable 메소드를 호출
        //3. gboard.MoveEnable 메소드는 보드 x,y 좌표와 도형 x,y 좌표를 비교 -> true 반환시 MoveTurn 반복
        //4. gboard.MoveEnable 메소드 false 
        internal bool MoveTurn()
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[brick.BlockArr[0], (Turn + 1) % 4, xx, yy] != 0)
                    {
                        if (((brick.X + xx) < 0) || ((brick.X + xx) >= GameScreen.BX) ||
                                ((brick.Y + yy) >= GameScreen.BY))
                        {
                            return false;
                        }
                    }
                }
            }
            if (gboard.MoveEnable(brick.BlockArr[0], (Turn + 1) % 4, brick.X, brick.Y))
            {
                brick.MoveTurn();
                return true;
            }
            return false;
        }
        ```
        
    - **블럭의 모양 (편의상 1자 막대만 가져옴)**
        
        블록의 형태를 정의하는 4차원 배열이다.
        
        각 블록에 대한 4가지 회전 상태를 나타낸다. 블록은 4 * 4크기의 격자로 표현된다.
        
        1은 블록이고, 0 은 빈 공간을 나타낸다.
        
        ```csharp
        static public readonly int[,,,] bvals = new int[9, 4, 4, 4]
        {
            {
                {  {0,0,1,0},
                   {0,0,1,0},
                   {0,0,1,0},
                   {0,0,1,0}},
                                // 1자 막대     □□■□        □□□□        □□■□        □□■□
                {  {0,0,0,0},
                   {0,0,0,0},
                   {1,1,1,1},
                   {0,0,0,0}},
                                //              □□■□  ->    □□□□ ->     □□■□ ->     □□■□
                {  {0,0,1,0},
                   {0,0,1,0},
                   {0,0,1,0},
                   {0,0,1,0}}, 
                                //              □□■□        ■■■■        □□■□        □□■□
                {  {0,0,0,0},
                   {0,0,0,0},
                   {1,1,1,1},
                   {0,0,0,0}}         //              □□■□        □□□□        □□■□        □□■□
            },
        ```
        
    - **다음 블럭 미리보기 및 회전 랜덤**
        
        게임의 블록 초기화를 수행하는 함수이다.
        
        새로운 블록을 게임판의 초기 위치에 배치하고, 블록이 특정 방향 및 회전된 상태로 시작하도록 초기화 한다.
        
        ```csharp
        internal void Reset() //벽돌의 초기 위치
        {
            Random random = new Random();
            X = GameScreen.SX;
            Y = GameScreen.SY;
            Turn = random.Next() % 4;
        
            
            BlockArr[0] = BlockArr[1];
            if (Board.Level >= 4)
            {
                BlockArr[1] = random.Next() % 9;
            }
            else
            {
                BlockArr[1] = random.Next() % 7;
            }
        }
        ```
        
    - **블럭 쌓기**
        
        테트리스 게임에서 현재 위치에 있는 블록의 값을 보드에 저장하고, 해당 라인이 꽉 찼는지 확인하는 함수이다.
        
        ```csharp
        /* 1. 벽돌의 4*4 공간을 하나씩 확인
         * 2. 만약 해당 공간의 x,y 좌표와 현재 벽돌이 보드에 있는 x,y 좌표를 적용한 값이 보드 공간 내에 있을 때 보드의 값과 벽돌의 값을 더한다.
         * 3. 그 값을 보드에 저장한다.
         * 4. 라인이 꽉 찼는지 ChekLines 메소드 호출(벽돌이 4*4 공간에 배치하므로 벽돌 있는 좌표에서 3칸 더 있다는 것을 고려)
         */
        internal void Store(int bn, int turn, int x, int y)
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (((x + xx) >= 0) && (x + xx < GameScreen.BX) && (y + yy >= 0) && (y + yy < GameScreen.BY))
                    {
                        board[x + xx, y + yy] += BlockValue.bvals[bn, turn, xx, yy];
                    }
                }
            }
            CheckLines(y + 3);
        }
        ```
        
    - **블럭 예상 위치**
        
        벽돌이 특정 위치로 이동할 수 있는지를 확인하는 함수이다.
        
        ```csharp
            internal bool IsValidMove(int cx, int cy)
            {
                Point now = BrickPoint;
                int bn = BlockArr[0];
                int tn = Turn;
        
                // 이동한 위치에서의 블록의 상태를 확인
                for (int xx = 0; xx < 4; xx++)
                {
                    for (int yy = 0; yy < 4; yy++)
                    {
                        if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                        {
                            int newX = now.X + cx + xx;
                            int newY = now.Y + cy + yy;
        
                            // 이동한 위치가 보드 내부인지 확인
                            if (newX < 0 || newX >= GameScreen.BX || newY < 0 || newY >= GameScreen.BY)
                            {
                                return false;
                            }
        
                            // 이동한 위치에 다른 블록이 이미 있는지 확인
                            if (this[newX, newY] != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
        
                return true; // 이동 가능
            }
        
        }
        ```
        
    - **블럭이 보드의 공간을 벗어났는지 확인**
        
        y 좌표에 대해 해당 라인이 모두 채워졌는지 여부를 확인하는 함수이다.
        
        ```csharp
        // 
        private bool CheckLine(int y)
        {
            for (int xx = 0; xx < GameScreen.BX; xx++)
            {
                if (board[xx, y] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        ```
        
    - **블럭 지우기**
        
        y 좌표에서 위로 올라가면 라인이 꽉 찼는지 확인하고, 꽉 찬 라인이 있으면 해당 라인을 지우고 점수를 올리고 레벨업을 확인하는 함수이다.
        
        y 좌표에 있는 라인을 지우고, 그 위의 모든 라인을 한 칸씩 아래로 내리는 함수이다.
        
        ```csharp
        private void CheckLines(int y) 
        {
            int yy = 0;
            for (yy = 0; yy < 4; yy++)
            {
                if (y - yy < GameScreen.BY)
                {
                    if (CheckLine(y - yy))
                    {
                        ClearLine(y - yy);
                        y++;
                        score += 10;
                        LevelUp();
                    }
                }
            }
        }
        
        private void ClearLine(int y)
        {
            for (; y > 0; y--)
            {
                for (int xx = 0; xx < GameScreen.BX; xx++)
                {
                    board[xx, y] = board[xx, y - 1];
                }
            }
        }
        ```
        
    - **기본 폼 구성**
        
        테트리스 게임의 현재 블록을 그리는 함수이다.
        
        ```csharp
        // 벽돌 생성
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
        ```
        
        테트리스 도형이 이동할 때 GUI를 업데이트하기 위한 Region을 생성하는 함수이다.
        
        ```csharp
        // 도형 방향 이동 시 GUI Update
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
        ```
        
        테트리스 도형이 회전할 때 GUI를 업데이트 하기 위한 Region을 생성하는 함수이다.
        
        ```csharp
        
        // 도형을 회전 시 GUI 업데이트
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
        ```
        
    - **데이터 연결 (데이터 베이스)**
        
        플레이어의 점수를 데이터 베이스에 저장하는 메소드이다.
        
        ```csharp
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
        ```
        
        데이터 베이스에서 순위를 계산하여 정렬된 데이터를 가져와서 DataGridView에 로드하는 메소드이다.
        
        ```csharp
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
        ```
        
    - **레벨**
        
        플레이어의 점수가 특정 값에 도달할 때마다 게임의 레벨을 업데이트하는 메소드이다.
        
        ```csharp
        private void LevelUp()
        {
            if (score == 10) Level = 1;
            if (score == 20) Level = 2;
            if (score == 30) Level = 3;
            if (score == 40) Level = 4;
            if (score == 50) Level = 5;
        }
        ```
        
        게임에서 레벨이 변경되거나 특정 상황에 도달했을 때 폼의 배경색을 깜빡이게 만드는 기능이다.
        
        ```csharp
        // 레벨 - 화면 플레쉬
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
        ```
        
        일정한 간격으로 ‘MoveDown()’ 메소드를 호출하여 테트리스 블록을 아래로 이동시키며, 레벨에 따라 타이머의 간격을 조절하여 난이도를 조절한다.
        
        ```csharp
        // 레벨 - 속도
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
        ```
        
- 전체 코드
    - GameScreen.cs
        
        ```csharp
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
        
        ```
        
    - Tetriss.cs
        
        ```csharp
        using System;
        using System.Collections.Generic;
        using System.Drawing;
        using System.Linq;
        using System.Runtime.Remoting.Messaging;
        using System.Text;
        using System.Threading.Tasks;
        using System.Windows.Forms;
        using Tetris_02;
        
        namespace Tetriss_02
        {
            internal class Tetriss
            {
                Brick brick;
                Board gboard = Board.GameBoard;
                public int NextBlockNum { get; private set; }
        
                public void SetNextBlock()
                {
                    NextBlockNum = brick.BlockNum + 1;
                }
                internal Point BrickPoint //벽돌의 좌표 가져오기
                {
                    get
                    {
                        return new Point(brick.X, brick.Y);
                    }
                }
                internal int BlockNum
                {
                    get
                    {
                        return brick.BlockNum;
                    }
                }
                internal int[] BlockArr
                {
                    get
                    {
                        return brick.BlockArr;
                    }
                }
                internal int Turn
                {
                    get
                    {
                        return brick.Turn;
                    }
                }
                internal static Tetriss tetirss //객체 생성
                {
                    get;
                    private set;
                }
                internal int this[int x, int y]
                {
                    get
                    {
                        return gboard[x, y];
                    }
                }
                static Tetriss() //생성자 초기화
                {
                    tetirss = new Tetriss();
                }
                Tetriss() //Brick 호출 -> 생성
                {
                    brick = new Brick();
                }
        
                //벽돌의 움직임
        
                /* 
                키보드 좌/우/아래/위 화살표를 입력받아 함수 실행 
                보드의 x,y 좌표(xx, yy)와 도형의 x,y(brick.X, brick.Y) 좌표를 비교해
                쌓여있는 도형과의 겹침 방지 ,이동 시 게임 화면 밖으로 나가는거 방지 여부를 false , true 반환
                */
        
                //벽돌이 이동 가능한지 확인
                internal bool MoveLeft()
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (BlockValue.bvals[brick.BlockArr[0], Turn, xx, yy] != 0) //쌓여있는 도형과의 겹침 방지
                            {
                                if (brick.X + xx <= 0) //회전 시 게임화면 왼쪽 밖으로 나가는거 방지
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X - 1, brick.Y))
                    {
                        brick.MoveLeft();
                        return true;
                    }
                    return false;
                }
                internal bool MoveRight()
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (BlockValue.bvals[brick.BlockArr[0], Turn, xx, yy] != 0)
                            {
                                if ((brick.X + xx + 1) >= GameScreen.BX) //회전 시 게임화면 오른쪽 밖으로 나가는거 방지
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X + 1, brick.Y))
                    {
                        brick.MoveRight();
                        return true;
                    }
                    return false;
                }
                //1. timer 이벤트를 이용하여 일정 시간마다 MoveDown 메소드 호출
                //2. MoveDown 메소드 실행 시 gboard.MoveEnable 메소드를 호출
                //3. gboard.MoveEnable 메소드는 보드 x,y 좌표와 도형 x,y 좌표를 비교 -> true 반환시 MoveDown 반복
                //4. gboard.MoveEnable 메소드 false 반환 시 gboard.Store 함수 호출
                //5. gboard.Store 호출하여 해당 좌표 보드 영역에 도형의 데이터값 대입
                internal bool MoveDown()
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (BlockValue.bvals[brick.BlockArr[0], Turn, xx, yy] != 0)
                            {
                                if ((brick.Y + yy + 1) >= GameScreen.BY)
                                {
                                    gboard.Store(brick.BlockArr[0], Turn, brick.X, brick.Y);
                                    return false;
                                }
                            }
                        }
                    }
                    if (gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X, brick.Y + 1))
                    {
                        brick.MoveDown();
                        return true;
                    }
                    gboard.Store(brick.BlockArr[0], Turn, brick.X, brick.Y);
                    return false;
                }
                //1. timer 이벤트를 이용하여 일정 시간마다 MoveTurn 메소드 호출
                //2. MoveTurn 메소드 실행 시 gboard.MoveEnable 메소드를 호출
                //3. gboard.MoveEnable 메소드는 보드 x,y 좌표와 도형 x,y 좌표를 비교 -> true 반환시 MoveTurn 반복
                //4. gboard.MoveEnable 메소드 false 
                internal bool MoveTurn()
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (BlockValue.bvals[brick.BlockArr[0], (Turn + 1) % 4, xx, yy] != 0)
                            {
                                if (((brick.X + xx) < 0) || ((brick.X + xx) >= GameScreen.BX) ||
                                        ((brick.Y + yy) >= GameScreen.BY))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (gboard.MoveEnable(brick.BlockArr[0], (Turn + 1) % 4, brick.X, brick.Y))
                    {
                        brick.MoveTurn();
                        return true;
                    }
                    return false;
                }
                internal bool Next() //벽돌을 아래로 다 이동 시 다음 블럭 호출
                {
                    brick.Reset();
                    SetNextBlock();
                    return gboard.MoveEnable(brick.BlockArr[0], Turn, brick.X, brick.Y);
                }
        
                //internal void lineline() //한 줄 랜덤
                //{
                //    brick.LineRandom();
                //}
        
                internal void ReStart()
                {
                    Board.score = 0;
                    Board.Level = 0;
                    gboard.ClearBoard();
                }
                internal bool IsValidMove(int cx, int cy)
                {
                    Point now = BrickPoint;
                    int bn = BlockArr[0];
                    int tn = Turn;
        
                    // 이동한 위치에서의 블록의 상태를 확인
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                            {
                                int newX = now.X + cx + xx;
                                int newY = now.Y + cy + yy;
        
                                // 이동한 위치가 보드 내부인지 확인
                                if (newX < 0 || newX >= GameScreen.BX || newY < 0 || newY >= GameScreen.BY)
                                {
                                    return false;
                                }
        
                                // 이동한 위치에 다른 블록이 이미 있는지 확인
                                if (this[newX, newY] != 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
        
                    return true; // 이동 가능
                }
        
            }
        }
        ```
        
    - Brick.cs
        
        ```csharp
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading;
        using System.Threading.Tasks;
        using Tetris_02;
        
        namespace Tetriss_02
        {
            internal class Brick
            {
                internal int X //도형의 X좌표, Y좌표 가져오는 함수
                {
                    get;
                    private set;
                }
                internal int Y
                {
                    get;
                    private set;
                }
                internal int Turn //블럭 회전
                {
                    get;
                    private set;
                }
                internal int BlockNum //도형 숫자 (7가지)
                {
                    get;
                    private set;
                }
                internal int[] BlockArr
                {
                    get;
                    private set;
                }
                internal Brick() //생성자
                {
                    BlockArr = new int[2];
                    Reset();
                    //LineRandom();
                }
        
                //private bool isReset = false;
                internal void Reset() //벽돌의 초기 위치
                {
                    Random random = new Random();
                    X = GameScreen.SX;
                    Y = GameScreen.SY;
                    Turn = random.Next() % 4;
        
                    
                    BlockArr[0] = BlockArr[1];
                    if (Board.Level >= 4)
                    {
                        BlockArr[1] = random.Next() % 9;
                    }
                    else
                    {
                        BlockArr[1] = random.Next() % 7;
                    }
        
                }
                //if (isReset == true)
                //    {
                //    if (isReset == false)
                //    {
                //        for (int i = 0; i < 2; i++)
                //        {
                //            BlockArr[i] = random.Next() % 9;
                //        }
                //        isReset = true;
                //    }
                //}
        
                //internal void LineRandom()
                //{
                //    Random random = new Random();
                //    Console.Write("DisLine ");
                //    for (int i = 0; i < BlockValue.DisLine.Length; i++)
                //    {
                //        BlockValue.DisLine[i] = random.NextDouble() < 0.8 ? 1 : 0;
                //    }
                //}
        
                //벽돌 움직이는 함수
                internal void MoveLeft()
                {
                    X--;
                }
                internal void MoveRight()
                {
                    X++;
                }
                internal void MoveDown()
                {
                    Y++;
                }
                internal void MoveTurn() //4번 회전
                {
                    Turn = (Turn + 1) % 4;
                }
            }
        }
        
        ```
        
    - Board.cs
        
        ```csharp
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Net.Sockets;
        using System.Text;
        using System.Threading.Tasks;
        using Tetris_02;
        
        namespace Tetriss_02
        {
            class Board //보드의 상태를 기억
            {
                internal static Board GameBoard
                {
                    get;
                    private set;
                }
                static Board()
                {
                    GameBoard = new Board();
                }
                Board()
                {
                }
                int[,] board = new int[GameScreen.BX, GameScreen.BY];
                public static int score = 0; // 예: 점수 
                public static int Level = 0; // 레벨
                internal int this[int x, int y] //보드의 특정 영역이 어떤 값인지 확인할 수 있기 위해 만듦
                {
                    get
                    {
                        return board[x, y];
                    }
                }
        
                internal bool MoveEnable(int bn, int tn, int x, int y) //벽돌이 이동 가능한지 확인
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                            {
                                if (board[x + xx, y + yy] != 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
                internal void Store(int bn, int turn, int x, int y)
                {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        for (int yy = 0; yy < 4; yy++)
                        {
                            if (((x + xx) >= 0) && (x + xx < GameScreen.BX) && (y + yy >= 0) && (y + yy < GameScreen.BY))
                            {
                                board[x + xx, y + yy] += BlockValue.bvals[bn, turn, xx, yy];
                            }
                        }
                    }
                    CheckLines(y + 3);
                }
                private void CheckLines(int y) // 한줄 지워지는거
                {
                    int yy = 0;
                    for (yy = 0; yy < 4; yy++)
                    {
                        if (y - yy < GameScreen.BY)
                        {
                            if (CheckLine(y - yy))
                            {
                                ClearLine(y - yy);
                                y++;
                                score += 10;
                                LevelUp();
                            }
                        }
                    }
                }
                private void LevelUp()
                {
                    if (score == 10) Level = 1;
                    if (score == 20) Level = 2;
                    if (score == 30) Level = 3;
                    if (score == 40) Level = 4;
                    if (score == 50) Level = 5;
                }
                private void ClearLine(int y)
                {
                    for (; y > 0; y--)
                    {
                        for (int xx = 0; xx < GameScreen.BX; xx++)
                        {
                            board[xx, y] = board[xx, y - 1];
                        }
                    }
                }
                private bool CheckLine(int y)
                {
                    for (int xx = 0; xx < GameScreen.BX; xx++)
                    {
                        if (board[xx, y] == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                internal void ClearBoard()
                {
                    for (int xx = 0; xx < GameScreen.BX; xx++)
                    {
                        for (int yy = 0; yy < GameScreen.BY; yy++)
                        {
                            board[xx, yy] = 0;
                        }
                    }
                }
            }
        }
        
        ```
        
    - BlockValue.cs
        
        ```csharp
        using System;
        using System.Collections.Generic;
        using System.Drawing;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        
        namespace Tetriss_02
        {
            internal class BlockValue
            {
                public enum BlockColor
                {
                    yello, yellowgreen, Green, Pink, SkyBlue, Gray, White
                }
                private static readonly Dictionary<BlockColor, Color> blockColors = new Dictionary<BlockColor, Color>
                {
                    {BlockColor.yello, Color.Yellow },
                    {BlockColor.yellowgreen, Color.YellowGreen },
                    {BlockColor.Green, Color.Green },
                    {BlockColor.Pink, Color.Pink },
                    {BlockColor.SkyBlue, Color.SkyBlue },
                    {BlockColor.Gray, Color.Gray },
                    {BlockColor.White, Color.White }
                };
                static public readonly int[,,,] bvals = new int[9, 4, 4, 4]
                {
                    {
                        {  {0,0,1,0},{0,0,1,0},{0,0,1,0},{0,0,1,0}},        // 1자 막대     □□■□        □□□□        □□■□        □□■□
                        {  {0,0,0,0},{0,0,0,0},{1,1,1,1},{0,0,0,0}},        //              □□■□  ->    □□□□ ->     □□■□ ->     □□■□
                        {  {0,0,1,0},{0,0,1,0},{0,0,1,0},{0,0,1,0}},        //              □□■□        ■■■■        □□■□        □□■□
                        {  {0,0,0,0},{0,0,0,0},{1,1,1,1},{0,0,0,0}}         //              □□■□        □□□□        □□■□        □□■□
                    },
                    {
                        {  {0,0,0,0},{0,1,1,0},{0,1,1,0},{0,0,0,0}},        // 네모 블럭    □□□□
                        {  {0,0,0,0},{0,1,1,0},{0,1,1,0},{0,0,0,0}},        //              □■■□
                        {  {0,0,0,0},{0,1,1,0},{0,1,1,0},{0,0,0,0}},        //              □■■□
                        {  {0,0,0,0},{0,1,1,0},{0,1,1,0},{0,0,0,0}}         //              □□□□
                    },
                    {
                        {  {0,0,0,0},{0,1,1,0},{0,0,1,0},{0,0,1,0}},        // ㄱ자 Left    □□□□        □□□□        □□□□        □□□□
                        {  {0,0,0,0},{0,0,0,1},{0,1,1,1},{0,0,0,0}},        //              □■■□  ->    □□□■ ->     □□■□ ->     □□□□ 
                        {  {0,0,0,0},{0,0,1,0},{0,0,1,0},{0,0,1,1}},        //              □□■□        □■■■        □□■□        □■■■
                        {  {0,0,0,0},{0,0,0,0},{0,1,1,1},{0,1,0,0}}         //              □□■□        □□□□        □□■■        □■□□
                    },
                    {
                        {  {0,0,0,0},{0,0,1,1},{0,0,1,0},{0,0,1,0}},        // ㄱ자 Right   □□□□        □□□□        □□□□        □□□□  
                        {  {0,0,0,0},{0,0,0,0},{0,1,1,1},{0,0,0,1}},        //              □□■■  ->    □□□□ ->     □□■□ ->     □■□□  
                        {  {0,0,0,0},{0,0,1,0},{0,0,1,0},{0,1,1,0}},        //              □□■□        □■■■        □□■□        □■■■
                        {  {0,0,0,0},{0,1,0,0},{0,1,1,1},{0,0,0,0}}         //              □□■□        □■□□        □■■□        □□□□
                    },
                    {
                        {  {0,0,1,0},{0,0,1,1},{0,0,1,0},{0,0,0,0}},        //  ㅏ자 블럭
                        {  {0,0,0,0},{0,1,1,1},{0,0,1,0},{0,0,0,0}},        //
                        {  {0,0,0,0},{0,0,1,0},{0,1,1,0},{0,0,1,0}},        //
                        {  {0,0,1,0},{0,1,1,1},{0,0,0,0},{0,0,0,0}}         //
                    },
                    {
                        {  {0,0,0,0},{0,1,1,0},{0,0,1,1},{0,0,0,0}},        //   z자 블럭
                        {  {0,0,0,0},{0,0,0,1},{0,0,1,1},{0,0,1,0}},        //   Left
                        {  {0,0,0,0},{0,1,1,0},{0,0,1,1},{0,0,0,0}},        //
                        {  {0,0,0,0},{0,0,0,1},{0,0,1,1},{0,0,1,0}}         //
                    },
                    {
                        {  {0,0,0,0},{0,0,1,1},{0,1,1,0},{0,0,0,0}},        //   z자 블럭
                        {  {0,0,0,0},{0,0,1,0},{0,0,1,1},{0,0,0,1}},        //   Right
                        {  {0,0,0,0},{0,0,1,1},{0,1,1,0},{0,0,0,0}},        //
                        {  {0,0,0,0},{0,0,1,0},{0,0,1,1},{0,0,0,1}}         //
                    },
                    {
                        {  {0,0,1,0},{0,1,1,1},{0,0,1,0},{0,0,0,0}},        //   +자 블럭
                        {  {0,0,1,0},{0,1,1,1},{0,0,1,0},{0,0,0,0}},        //   Right
                        {  {0,0,1,0},{0,1,1,1},{0,0,1,0},{0,0,0,0}},        //
                        {  {0,0,1,0},{0,1,1,1},{0,0,1,0},{0,0,0,0}}
                    },
                    {
                        {  {0,1,0,1},{0,1,1,1}, {0,0,0,0},{0,0,0,0}},       //   ㄷ자 블럭
                        {  {0,0,1,1},{0,0,1,0},{0,0,1,1},{0,0,0,0}},        //   Right
                        {  {0,1,1,1},{0,1,0,1},{0,0,0,0},{0,0,0,0}},        //
                        {  {0,0,1,1},{0,0,0,1},{0,0,1,1},{0,0,0,0}}
                    }
                };
                static public readonly BlockColor[] blockColorsArr = new BlockColor[]  //블록 색깔 정의
                {
                    BlockColor.Pink,
                    BlockColor.SkyBlue,
                    BlockColor.yello,
                    BlockColor.yello,
                    BlockColor.Green,
                    BlockColor.yellowgreen,
                    BlockColor.yellowgreen,
                    BlockColor.White,
                    BlockColor.White
                };
                public static Color GetBlockColor(BlockColor color)
                {
                    if (blockColors.ContainsKey(color))
                    {
                        return blockColors[color];
                    }
                    else
                    {
                        return Color.Black;
                    }
                }
        
                static public readonly int[] DisLine = new int[12]
                    {0,0,0,0,0,0,0,0,0,0,0,0};
            }
        
        }
        
        ```
        

## 5. 프로젝트 성과 결과

- C# 윈폼으로 테트리스 게임을 구현
- 다음 도형 미리보기 및 예상 위치 표시를 구현
- 점수 계산 및 레벨 구현
- 레벨에 따른 난이도 변화 및 화면 효과
- 점수 및 이름 저장, 랭킹 확인을 데이터 베이스와 접목

## 6. 아쉬운점

1. 하단에서 올라오는 블록 및 게임 아이템 구현 실패
2. 시간이 부족하여 최고 점수를 게임 화면에 표시하는 것을 못함
3. 2인 이상 가능한 멀티 및 멀티 플레이 중 아이템 사용을 구현하지 못함

## 7. 참고

[[C# 프로젝트] 테트리스 만들기 – Part 1. 키보드로 도형 제어하기, 타이머로 도형 아래로 이동 - 언제나 휴일](https://ehpub.co.kr/c-프로젝트-테트리스-만들기-part-1-키보드로-도형-제어하/)

## 8. 시연
