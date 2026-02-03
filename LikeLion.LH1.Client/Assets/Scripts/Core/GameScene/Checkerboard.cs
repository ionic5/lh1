using LikeLion.LH1.Client.Core.View.GameScene;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class Checkerboard : ICheckerboard
    {
        private readonly List<List<int>> _board;
        private readonly View.GameScene.ICheckerboard _checkerboardView;
        private readonly Core.ILogger _logger;
        private bool _isDestroyed;
        private string _gameGuid;
        private List<Tuple<string, int>> _stoneOwners;

        public event EventHandler<StonePointClickedEventArgs> StonePointClickedEvent;
        public event EventHandler<StonePuttedEventArgs> StonePuttedEvent;

        public Checkerboard(View.GameScene.ICheckerboard checkerboardView, ILogger logger)
        {
            _stoneOwners = new List<Tuple<string, int>>();

            _checkerboardView = checkerboardView;
            _checkerboardView.StonePointClickedEvent += OnStonePointClickedEvent;
            _checkerboardView.DestroyEvent += OnDestroyViewEvent;

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

        private void OnDestroyViewEvent(object sender, DestroyEventArgs e)
        {
            Destroy();
        }

        public void Destroy()
        {
            if (_isDestroyed)
                return;
            _isDestroyed = true;

            _checkerboardView.StonePointClickedEvent -= OnStonePointClickedEvent;
            _checkerboardView.DestroyEvent -= OnDestroyViewEvent;
            _checkerboardView.Destroy();

            _board.Clear();
        }

        private void OnStonePointClickedEvent(object sender, View.GameScene.StonePointClickedEventArgs args)
        {
            StonePointClickedEvent?.Invoke(this, new StonePointClickedEventArgs
            {
                Row = args.Row,
                Column = args.Column
            });
        }

        public int[][] ToArray()
        {
            return _board.Select(row => row.ToArray()).ToArray();
        }

        public void PutStone(int column, int row, int stoneType)
        {
            if (_board[column][row] != StoneType.Null)
            {
                _logger.Warn("Attempted to place a stone on a non-empty point. Ignored.");
                return;
            }

            _board[column][row] = stoneType;
            _checkerboardView.PutStone(column, row, stoneType);

            StonePuttedEvent?.Invoke(this, new StonePuttedEventArgs { StoneType = stoneType });
            return;
        }

        public void Clear()
        {
            for (int i = 0; i < 19; i++)
                for (int j = 0; j < 19; j++)
                    _board[i][j] = StoneType.Null;

            _checkerboardView.Clear();

            _stoneOwners.Clear();
        }

        public bool IsStonePointEmpty(int column, int row)
        {
            return _board[column][row] == StoneType.Null;
        }

        public string GetGameGuid()
        {
            return _gameGuid;
        }

        public void SetGameGuid(string gameGuid)
        {
            _gameGuid = gameGuid;
        }

        public void RegisterStoneOwner(string playerGuid, int stoneType)
        {
            _stoneOwners.Add(new Tuple<string, int>(playerGuid, stoneType));
        }

        public int GetStone(string playerGuid)
        {
            return _stoneOwners.Where(entry => entry.Item1 == playerGuid).Select(entry => entry.Item2).First();
        }
    }
}
