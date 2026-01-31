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
        private readonly Core.ILogger _logger;

        public event EventHandler<StonePointClickedEventArgs> StonePointClickedEvent;
        public event EventHandler<StonePuttedEventArgs> StonePuttedEvent;

        public Checkerboard(ICheckerboard checkerboardView, ILogger logger)
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

            _logger = logger;
        }

        public int[][] ToArray()
        {
            return _board.Select(row => row.ToArray()).ToArray();
        }

        public void PutStone(int column, int row, int stoneType)
        {
            if (_board[column][row] != StoneType.Null)
            {
                _logger.Fatal($"Stone already exists at this position. Current board state : {ToArray()} StoneType : {stoneType}");
                return;
            }

            _board[column][row] = stoneType;
            _checkerboardView.PutStone(column, row, stoneType);

            StonePuttedEvent?.Invoke(this, new StonePuttedEventArgs { StoneType = stoneType });
        }

        public void Clear()
        {
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 19; j++)
                    _board[i][j] = StoneType.Null;

            _checkerboardView.Clear();
        }
    }
}
