using System;
using System.Collections.Generic;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class MockGameSession : IGameSession
    {
        public event EventHandler<ConnectedEventArgs> ConnectedEvent;
        public event EventHandler<GameCreatedEventArgs> GameCreatedEvent;
        public event EventHandler<GamePreparedEventArgs> GamePreparedEvent;
        public event EventHandler<PlayerTurnStartedEventArgs> PlayerTurnStartedEvent;
        public event EventHandler<PlayerTurnFinishedEventArgs> PlayerTurnFinishedEvent;
        public event EventHandler<GameFinishedEventArgs> GameFinishedEvent;

        private readonly Core.Timer _timer;
        private readonly float _timeLimit;
        private readonly Entity.Checkerboard _checkerboard;
        private IPlayer _dummyPlayer;

        public MockGameSession(Timer timer, Entity.Checkerboard checkerboard)
        {
            _timeLimit = 8;
            _timer = timer;
            _checkerboard = checkerboard;
        }

        public void RequestConnect()
        {
            var playerGuid = Guid.NewGuid().ToString();

            ConnectedEvent?.Invoke(this, new ConnectedEventArgs { PlayerGuid = playerGuid });
        }

        public void RequestGame(string playerGuid)
        {
            var dummyPlayerGuid = Guid.NewGuid().ToString();
            _dummyPlayer.SetPlayerGuid(dummyPlayerGuid);

            var gameGuid = Guid.NewGuid().ToString();
            _checkerboard.SetGameGuid(gameGuid);
            _checkerboard.Setup();

            GameCreatedEvent?.Invoke(this, new GameCreatedEventArgs { GameGuid = gameGuid });
        }

        public void PickStone(string gameGuid, string playerGuid, int stoneType)
        {
            _checkerboard.RegisterStoneOwner(playerGuid, stoneType);

            var dummyPlayerStoneType = StoneType.Black == stoneType ? StoneType.White : StoneType.Black;
            _checkerboard.RegisterStoneOwner(_dummyPlayer.GetPlayerGuid(), dummyPlayerStoneType);

            var stoneOwners = new List<StoneOwner>
            {
                new StoneOwner { PlayerGuid = playerGuid, StoneType = stoneType },
                new StoneOwner { PlayerGuid = _dummyPlayer.GetPlayerGuid(), StoneType = dummyPlayerStoneType }
            };
            GamePreparedEvent?.Invoke(this, new GamePreparedEventArgs { StoneOwners = stoneOwners });
        }

        public void StartGame(string gameGuid, string playerGuid)
        {
            PlayerTurnStartedEvent += OnPlayerTurnStartedEvent;
            PlayerTurnFinishedEvent += OnPlayerTurnFinishedEvent;

            var firstTurnPlayerGuid = _checkerboard.GetPlayerGuid(StoneType.Black);
            StartTurn(firstTurnPlayerGuid);
        }

        private void StartTurn(string playerGuid)
        {
            PlayerTurnStartedEvent?.Invoke(this, new PlayerTurnStartedEventArgs
            {
                PlayerGuid = playerGuid,
                TimeLimit = _timeLimit
            });

            _timer.Start(0, _timeLimit, () =>
            {
                PlayerTurnFinishedEvent?.Invoke(this, new PlayerTurnFinishedEventArgs
                {
                    PlayerGuid = playerGuid,
                    StoneType = StoneType.Null,
                    Column = -1,
                    Row = -1
                });

                var opponentPlayerGuid = _checkerboard.GetOpponentPlayerGuid(playerGuid);
                StartTurn(opponentPlayerGuid);
            });
        }

        public void PutStone(string gameGuid, string playerGuid, int column, int row)
        {
            var stoneType = _checkerboard.GetStone(playerGuid);

            _checkerboard.PutStone(column, row, stoneType);

            _timer.Stop(0);

            PlayerTurnFinishedEvent?.Invoke(this, new PlayerTurnFinishedEventArgs
            {
                PlayerGuid = playerGuid,
                StoneType = stoneType,
                Column = column,
                Row = row
            });

            var winnerStone = _checkerboard.CheckWinner();
            if (winnerStone == StoneType.Null)
            {
                var opponentPlayerGuid = _checkerboard.GetOpponentPlayerGuid(playerGuid);
                StartTurn(opponentPlayerGuid);
            }
            else
            {
                PlayerTurnStartedEvent -= OnPlayerTurnStartedEvent;
                PlayerTurnFinishedEvent -= OnPlayerTurnFinishedEvent;

                var winnerGuid = _checkerboard.GetPlayerGuid(winnerStone);
                _checkerboard.Clear();

                GameFinishedEvent?.Invoke(this, new GameFinishedEventArgs
                {
                    WinnerGuid = winnerGuid,
                    WinnerStone = winnerStone
                });
            }
        }

        private void OnPlayerTurnStartedEvent(object sender, PlayerTurnStartedEventArgs args)
        {
            if (args.PlayerGuid != _dummyPlayer.GetPlayerGuid())
                return;

            _dummyPlayer.StartTurn();
        }

        private void OnPlayerTurnFinishedEvent(object sender, PlayerTurnFinishedEventArgs args)
        {
            if (args.PlayerGuid != _dummyPlayer.GetPlayerGuid())
                return;

            _dummyPlayer.HaltTurn();
        }
    }
}
