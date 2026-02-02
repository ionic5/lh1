using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public interface IGameSession
    {
        event EventHandler<ConnectedEventArgs> ConnectedEvent;
        event EventHandler<GameCreatedEventArgs> GameCreatedEvent;
        event EventHandler GamePreparedEvent;
        event EventHandler<PlayerTurnStartedEventArgs> PlayerTurnStartedEvent;
        event EventHandler<PlayerTurnFinishedEventArgs> PlayerTurnFinishedEvent;
        event EventHandler<GameFinishedEventArgs> GameFinishedEvent;

        void PutStone(int column, int row);
        void RequestConnect();
        void RequestGame(string v);
        void StartGame(string gameGuid, string v);
    }
}
