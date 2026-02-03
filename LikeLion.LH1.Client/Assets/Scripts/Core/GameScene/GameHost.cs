using LikeLion.LH1.Client.Core.View.GameScene;
using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class GameHost : IUpdatable
    {
        private readonly IGameSession _gameSession;
        private readonly IPlayer _mainPlayer;
        private readonly ICheckerboard _checkerboard;
        private readonly Action<bool> _showResultPanel;
        private readonly Action _showPickStonePanel;
        private readonly IMainUIPanel _mainUIPanel;
        private readonly Core.Timer _timer;

        public GameHost(IGameSession gameSession, ICheckerboard checkerboard, IPlayer mainPlayer,
            IMainUIPanel mainUIPanel, Timer timer,
            Action<bool> showResultPanel, Action showPickStonePanel)
        {
            _gameSession = gameSession;
            _mainPlayer = mainPlayer;
            _checkerboard = checkerboard;
            _showResultPanel = showResultPanel;
            _showPickStonePanel = showPickStonePanel;
            _mainUIPanel = mainUIPanel;
            _timer = timer;
        }

        public void Connect()
        {
            _gameSession.ConnectedEvent += OnConnectedEvent;

            _gameSession.RequestConnect();
        }

        private void OnConnectedEvent(object sender, ConnectedEventArgs args)
        {
            _gameSession.ConnectedEvent -= OnConnectedEvent;

            _mainPlayer.SetPlayerGuid(args.PlayerGuid);

            RequestGame();
        }

        private void RequestGame()
        {
            Wait();

            _gameSession.RequestGame(_mainPlayer.GetPlayerGuid());
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

            _mainUIPanel.Show();
            _mainUIPanel.SetMainPlayerStone(_checkerboard.GetStone(_mainPlayer.GetPlayerGuid()));
        }

        private void OnPlayerTurnStartedEvent(object sender, PlayerTurnStartedEventArgs args)
        {
            var stoneType = _checkerboard.GetStone(args.PlayerGuid);

            _mainUIPanel.PlayTurnStartAnimation(stoneType);
            _mainUIPanel.SetCurrentPlayerStone(stoneType);
            _timer.Start(0, args.TimeLimit);

            if (_mainPlayer.GetPlayerGuid() != args.PlayerGuid)
                return;

            _mainPlayer.StartTurn();
        }

        private void OnPlayerTurnFinishedEvent(object sender, PlayerTurnFinishedEventArgs args)
        {
            _timer.Stop(0);

            if (args.StoneType != StoneType.Null)
                _checkerboard.PutStone(args.Column, args.Row, args.StoneType);

            if (_mainPlayer.GetPlayerGuid() != args.PlayerGuid)
                return;
            _mainPlayer.HaltTurn();
        }

        private void OnGameFinishedEvent(object sender, GameFinishedEventArgs args)
        {
            _timer.Stop(0);

            _mainUIPanel.Hide();

            bool isWinner = _mainPlayer.GetPlayerGuid() == args.WinnerGuid;
            _showResultPanel?.Invoke(isWinner);

            _gameSession.PlayerTurnStartedEvent -= OnPlayerTurnStartedEvent;
            _gameSession.PlayerTurnFinishedEvent -= OnPlayerTurnFinishedEvent;
            _gameSession.GameFinishedEvent -= OnGameFinishedEvent;
        }

        public void Restart()
        {
            _checkerboard.Clear();

            RequestGame();
        }

        public void Update()
        {
            if (_timer.IsRunning(0))
                _mainUIPanel.SetRemainTime(_timer.GetRemainTime(0));
        }
    }
}
