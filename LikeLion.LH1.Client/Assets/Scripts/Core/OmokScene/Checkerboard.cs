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
        private List<List<int>> _board;
        private ICheckerboard _checkerboardView;

        public Checkerboard(ICheckerboard checkerboardView)
        {
            _checkerboardView = checkerboardView;
            _board = new List<List<int>>();
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
