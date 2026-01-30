using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface IPickStonePanel
    {
        event EventHandler WhiteStoneButtonClickedEvent;
        event EventHandler BlackStoneButtonClickedEvent;

        void Hide();
    }
}
