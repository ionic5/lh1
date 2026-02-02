using System;

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

        public void PickStone(string gameGuid, string playerGuid, int stoneType)
        {
            throw new NotImplementedException();
        }

        public void PutStone(string gameGuid, string playerGuid, int column, int row)
        {
            throw new NotImplementedException();
        }

        public void RequestConnect()
        {
            throw new NotImplementedException();
        }

        public void RequestGame(string playerGuid)
        {
            throw new NotImplementedException();
        }

        public void StartGame(string gameGuid, string playerGuid)
        {
            throw new NotImplementedException();
        }
    }
}
