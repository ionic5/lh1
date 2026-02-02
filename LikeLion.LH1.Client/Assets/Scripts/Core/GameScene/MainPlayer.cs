using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class MainPlayer : IPlayer
    {
        private readonly Checkerboard _board;
        private readonly IGameSession _gameSession;
        private bool _isDestroyed;
        private bool _isMyTurn;
        private string _gameGuid;
        private string _playerGuid;
        private int _stoneType;

        public event EventHandler<DestroyEventArgs> DestroyEvent;

        public MainPlayer(Checkerboard board, IGameSession gameSession)
        {
            _board = board;
            _gameSession = gameSession;
            _isDestroyed = false;
            _isMyTurn = false;
            _playerGuid = string.Empty;
            _stoneType = StoneType.Null;

            _board.StonePointClickedEvent += OnStonePointClickedEvent;
        }

        public bool IsStoneOwner(int stoneType)
        {
            return _stoneType == stoneType;
        }

        public int GetStoneType()
        {
            return _stoneType;
        }

        public void SetStone(int stoneType)
        {
            _stoneType = stoneType;
        }

        public void StartTurn()
        {
            _isMyTurn = true;
        }

        public void HaltTurn()
        {
            _isMyTurn = false;
        }

        private void OnStonePointClickedEvent(object sender, StonePointClickedEventArgs args)
        {
            if (!_isMyTurn)
                return;

            var column = args.Column;
            var row = args.Row;
            if (_board.IsStonePointEmpty(column, row))
                _gameSession.PutStone(_gameGuid, _playerGuid, column, row);
        }

        public void Destroy()
        {
            if (_isDestroyed)
                return;
            _isDestroyed = true;

            DestroyEvent?.Invoke(this, new DestroyEventArgs(this));
            DestroyEvent = null;

            _isMyTurn = false;
            _board.StonePointClickedEvent -= OnStonePointClickedEvent;
        }

        public void SetPlayerGuid(string playerGuid)
        {
            _playerGuid = playerGuid;
        }

        public string GetPlayerGuid()
        {
            return _playerGuid;
        }

        public string GetGameGuid()
        {
            return _gameGuid;
        }

        public void SetGameGuid(string gameGuid)
        {
            _gameGuid = gameGuid;
        }
    }
}
