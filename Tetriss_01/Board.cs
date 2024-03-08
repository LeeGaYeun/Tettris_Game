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
