using LikeLion.LH1.Client.Core.View.OmokScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class ResultPanelController
    {
        private readonly OmokHost _omokHost;
        private readonly IResultPanel _panel;

        public ResultPanelController(OmokHost omokHost, IResultPanel panel)
        {
            _omokHost = omokHost;
            _panel = panel;

            _panel.RestartButtonClickedEvent += OnResultButtonClickedEvent;
        }

        public void OnResultButtonClickedEvent(object sender, EventArgs args)
        {
            _panel.RestartButtonClickedEvent -= OnResultButtonClickedEvent;
            _panel.Hide();

            _omokHost.Restart();
        }
    }
}
