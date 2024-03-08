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