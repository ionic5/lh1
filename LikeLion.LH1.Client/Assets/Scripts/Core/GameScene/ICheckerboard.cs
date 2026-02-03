using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public interface ICheckerboard
    {
        event EventHandler<StonePointClickedEventArgs> StonePointClickedEvent;

        string GetGameGuid();
        bool IsStonePointEmpty(int column, int row);
        void PutStone(int column, int row, int stoneType);
        void SetGameGuid(string gameGuid);
    }
}
