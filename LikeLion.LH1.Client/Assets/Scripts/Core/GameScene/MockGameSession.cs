using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class MockGameSession : IGameSession
    {
        public event EventHandler<ConnectedEventArgs> ConnectedEvent;
        public event EventHandler<GameCreatedEventArgs> GameCreatedEvent;
        public event EventHandler GamePreparedEvent;
        public event EventHandler<PlayerTurnStartedEventArgs> PlayerTurnStartedEvent;
        public event EventHandler<PlayerTurnFinishedEventArgs> PlayerTurnFinishedEvent;
        public event EventHandler<GameFinishedEventArgs> GameFinishedEvent;

        private List<Player> _players;

        private Core.Timer _timer;

        private class Player
        {
            public string PlayerGuid;
            public int StoneType;
        }

        public MockGameSession()
        {
            _players = new List<Player>();
        }

        public void PickStone(string gameGuid, string playerGuid, int stoneType)
        {
            var player = _players.First(entry => entry.PlayerGuid == playerGuid);
            player.StoneType = stoneType;

            var otherPlayer = _players.First(entry => entry.PlayerGuid != playerGuid);
            otherPlayer.StoneType = StoneType.Black == stoneType ? StoneType.White : StoneType.Black;

            GamePreparedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void PutStone(string gameGuid, string playerGuid, int column, int row)
        {
            var stoneType = _players.Where(entry => entry.PlayerGuid == playerGuid).Select(entry => entry.StoneType).First();

            _timer.Stop(0);

            PlayerTurnFinishedEvent?.Invoke(this, new PlayerTurnFinishedEventArgs
            {
                PlayerGuid = playerGuid,
                StoneType = stoneType,
                Column = column,
                Row = row
            });

            var otherPlayerGuid = _players.Where(entry => entry.PlayerGuid != playerGuid).Select(entry => entry.PlayerGuid).First();
            StartTurn(otherPlayerGuid);
        }

        public void RequestConnect()
        {
            var mainPlayer = new Player { PlayerGuid = Guid.NewGuid().ToString(), StoneType = StoneType.Null };
            _players.Add(mainPlayer);

            ConnectedEvent?.Invoke(this, new ConnectedEventArgs { PlayerGuid = mainPlayer.PlayerGuid });
        }

        public void RequestGame(string playerGuid)
        {
            _players.Add(new Player { PlayerGuid = Guid.NewGuid().ToString(), StoneType = StoneType.Null });

            GameCreatedEvent?.Invoke(this, new GameCreatedEventArgs { GameGuid = Guid.NewGuid().ToString() });
        }

        public void StartGame(string gameGuid, string playerGuid)
        {
            var player = _players.First(entry => entry.StoneType == StoneType.Black);

            StartTurn(player.PlayerGuid);
        }

        private void StartTurn(string playerGuid)
        {
            PlayerTurnStartedEvent?.Invoke(this, new PlayerTurnStartedEventArgs { PlayerGuid = playerGuid });

            _timer.Start(0, 60, () =>
            {
                PlayerTurnFinishedEvent?.Invoke(this, new PlayerTurnFinishedEventArgs
                {
                    PlayerGuid = playerGuid,
                    StoneType = StoneType.Null,
                    Column = -1,
                    Row = -1
                });
            });
        }
    }
}
