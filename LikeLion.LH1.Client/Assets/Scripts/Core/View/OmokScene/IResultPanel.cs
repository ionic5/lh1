using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface IResultPanel
    {
        event EventHandler RestartButtonClickedEvent;

        void Hide();
        void SetResult(bool v);
    }
}
