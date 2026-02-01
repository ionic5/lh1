using System;

namespace LikeLion.LH1.Client.Core.View.GameScene
{
    public interface ICheckerboard
    {
        event EventHandler<StonePointClickedEventArgs> StonePointClickedEvent;

        void PutStone(int column, int row, int stoneType);
        void Clear();
    }
}


