using System;

namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface IPickStonePanel
    {
        event EventHandler WhiteStoneButtonClickedEvent;
        event EventHandler BlackStoneButtonClickedEvent;

        void Hide();
    }
}
