using LikeLion.LH1.Client.Core.View.OmokScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class Checkerboard
    {
        private readonly List<List<int>> _board;
        private readonly ICheckerboard _checkerboardView;

        public Checkerboard(ICheckerboard checkerboardView)
        {
            _checkerboardView = checkerboardView;
            _board = new List<List<int>>();
            for (int i = 0; i < 19; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < 19; j++)
                    row.Add(StoneType.Null);
                _board.Add(row);
            }
        }

        public int[][] ToArray()
        {
            return _board.Select(row => row.ToArray()).ToArray();
        }

        public void PutStone(int column, int row, int stoneType)
        {
            _board[column][row] = stoneType;
            _checkerboardView.PutStone(column, row, stoneType);
        }
    }
}
