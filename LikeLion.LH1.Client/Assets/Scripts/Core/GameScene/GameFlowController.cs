using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class GameFlowController
    {
        private readonly IGameSession _gameSession;
        private readonly IPlayer _player;
        private readonly Action _showPickStonePanel;
        private readonly Action<bool> _showResultPanel;

        private string _gameGuid;
        private bool _isDestroyed;

        public void Start()
        {
            RemoveEventHandlers();

            _gameSession.ConnectedEvent += OnConnectedEvent;
            _gameSession.GameCreatedEvent += OnGameCreatedEvent;
            _gameSession.GamePreparedEvent += OnGamePreparedEvent;
            _gameSession.PlayerTurnStartedEvent += OnPlayerTurnStartedEvent;
            _gameSession.PlayerTurnFinishedEvent += OnPlayerTurnFinishedEvent;
            _gameSession.GameFinishedEvent += OnGameFinishedEvent;

            _gameSession.RequestConnect();
        }

        private void OnConnectedEvent(object sender, ConnectedEventArgs args)
        {
            _player.SetPlayerGuid(args.PlayerGuid);
            _gameSession.RequestGame(_player.GetPlayerGuid());
        }

        private void OnGameCreatedEvent(object sender, GameCreatedEventArgs args)
        {
            _gameGuid = args.GameGuid;
            _showPickStonePanel?.Invoke();
        }

        private void OnGamePreparedEvent(object sender, EventArgs args)
        {
            _gameSession.StartGame(_gameGuid, _player.GetPlayerGuid());
        }

        private void OnPlayerTurnStartedEvent(object sender, PlayerTurnStartedEventArgs args)
        {
            if (_player.GetPlayerGuid() != args.PlayerGuid)
                return;

            _player.StartTurn();
        }

        private void OnPlayerTurnFinishedEvent(object sender, PlayerTurnFinishedEventArgs args)
        {
            if (_player.GetPlayerGuid() != args.PlayerGuid)
                return;

            _player.HaltTurn();
        }

        private void OnGameFinishedEvent(object sender, GameFinishedEventArgs args)
        {
            bool isWinner = _player.GetPlayerGuid() == args.WinnerGuid;
            _showResultPanel?.Invoke(isWinner);

            RemoveEventHandlers();
        }

        private void RemoveEventHandlers()
        {
            _gameSession.ConnectedEvent -= OnConnectedEvent;
            _gameSession.GameCreatedEvent -= OnGameCreatedEvent;
            _gameSession.GamePreparedEvent -= OnGamePreparedEvent;
            _gameSession.PlayerTurnStartedEvent -= OnPlayerTurnStartedEvent;
            _gameSession.PlayerTurnFinishedEvent -= OnPlayerTurnFinishedEvent;
            _gameSession.GameFinishedEvent -= OnGameFinishedEvent;
        }

        public void Destroy()
        {
            if (_isDestroyed)
                return;
            _isDestroyed = true;

            RemoveEventHandlers();
        }
    }
}
