using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class GameHost
    {
        private readonly IGameSession _gameSession;
        private readonly IPlayer _mainPlayer;
        private readonly ICheckerboard _checkerboard;
        private readonly Action<bool> _showResultPanel;
        private readonly Action _showPickStonePanel;

        public GameHost(IGameSession gameSession, ICheckerboard checkerboard,
            IPlayer mainPlayer, Action<bool> showResultPanel, Action showPickStonePanel)
        {
            _gameSession = gameSession;
            _mainPlayer = mainPlayer;
            _checkerboard = checkerboard;
            _showResultPanel = showResultPanel;
            _showPickStonePanel = showPickStonePanel;
        }

        public void Wait()
        {
            _gameSession.GameCreatedEvent += OnGameCreatedEvent;
        }

        private void OnGameCreatedEvent(object sender, GameCreatedEventArgs args)
        {
            _gameSession.GameCreatedEvent -= OnGameCreatedEvent;

            _checkerboard.SetGameGuid(args.GameGuid);
            _showPickStonePanel?.Invoke();
        }

        public void Start()
        {
            _gameSession.PlayerTurnStartedEvent += OnPlayerTurnStartedEvent;
            _gameSession.PlayerTurnFinishedEvent += OnPlayerTurnFinishedEvent;
            _gameSession.GameFinishedEvent += OnGameFinishedEvent;

            _gameSession.StartGame(_checkerboard.GetGameGuid(), _mainPlayer.GetPlayerGuid());
        }

        private void OnPlayerTurnStartedEvent(object sender, PlayerTurnStartedEventArgs args)
        {
            if (_mainPlayer.GetPlayerGuid() != args.PlayerGuid)
                return;

            _mainPlayer.StartTurn();
        }

        private void OnPlayerTurnFinishedEvent(object sender, PlayerTurnFinishedEventArgs args)
        {
            if (args.StoneType != StoneType.Null)
                _checkerboard.PutStone(args.Column, args.Row, args.StoneType);

            if (_mainPlayer.GetPlayerGuid() != args.PlayerGuid)
                return;
            _mainPlayer.HaltTurn();
        }

        private void OnGameFinishedEvent(object sender, GameFinishedEventArgs args)
        {
            bool isWinner = _mainPlayer.GetPlayerGuid() == args.WinnerGuid;
            _showResultPanel?.Invoke(isWinner);

            _gameSession.PlayerTurnStartedEvent -= OnPlayerTurnStartedEvent;
            _gameSession.PlayerTurnFinishedEvent -= OnPlayerTurnFinishedEvent;
            _gameSession.GameFinishedEvent -= OnGameFinishedEvent;
        }
    }
}
