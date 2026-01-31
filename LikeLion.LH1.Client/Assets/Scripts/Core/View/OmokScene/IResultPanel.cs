using System;

namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface IResultPanel
    {
        event EventHandler RestartButtonClickedEvent;

        void Hide();
        void SetResult(bool v);
    }
}
