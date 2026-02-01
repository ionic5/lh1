using System;

namespace LikeLion.LH1.Client.Core.View.GameScene
{
    public interface IPickStonePanel
    {
        event EventHandler WhiteStoneButtonClickedEvent;
        event EventHandler BlackStoneButtonClickedEvent;

        void Hide();
    }
}
