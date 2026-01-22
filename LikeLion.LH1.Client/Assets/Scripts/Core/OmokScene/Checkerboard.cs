using LikeLion.LH1.Client.Core.View.OmokScene;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class Checkerboard
    {
        private readonly List<List<int>> _board;
        private readonly ICheckerboard _checkerboardView;

        public event EventHandler<StonePointClickedEventArgs> StonePointClickedEvent;
        public event EventHandler<StonePuttedEventArgs> StonePuttedEvent;

        public Checkerboard(ICheckerboard checkerboardView)
        {
            _checkerboardView = checkerboardView;
            _checkerboardView.StonePointClickedEvent += (sender, args) =>
            {
                StonePointClickedEvent?.Invoke(this, new StonePointClickedEventArgs
                {
                    Row = args.Row,
                    Column = args.Column
                });
            };

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

            StonePuttedEvent?.Invoke(this, new StonePuttedEventArgs { StoneType = stoneType });
        }
    }
}
