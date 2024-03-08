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
