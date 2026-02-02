using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public interface IGameSession
    {
        event EventHandler ConnectedEvent;
        event EventHandler GameStartedEvent;
        event EventHandler TurnStartedEvent;
        event EventHandler TurnFinishedEvent;
        event EventHandler GameFinishedEvent;
        event EventHandler StonePuttedEvent;
        event EventHandler PickStoneFinishedEvent;

        void PickStone(int stoneType);
        void RequestConnect();
        void PutStone();
        void Disconnect();
    }
}
