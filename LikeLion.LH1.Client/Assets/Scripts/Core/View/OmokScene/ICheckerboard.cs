

using Codice.Client.BaseCommands.BranchExplorer;
using System;

namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface ICheckerboard
    {
        event EventHandler<StonePointClickedEventArgs> StonePointClickedEvent;

        void PutStone(int column, int row);
    }
}


