using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class MockGameSession : IGameSession
    {
        public event EventHandler ConnectedEvent;
        public event EventHandler GameStartedEvent;
        public event EventHandler PlayerTurnStartedEvent;
        public event EventHandler TurnFinishedEvent;
        public event EventHandler GameFinishedEvent;
        public event EventHandler StonePuttedEvent;
        public event EventHandler PickStoneFinishedEvent;

        public void RequestConnect()
        {
            ConnectedEvent?.Invoke(this, EventArgs.Empty);

            GameStartedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void Disconnect()
        {
        }

        public void PutStone()
        {
        }

        public void PickStone(int white)
        {
            throw new NotImplementedException();
        }
    }
}
