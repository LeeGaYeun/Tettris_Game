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
