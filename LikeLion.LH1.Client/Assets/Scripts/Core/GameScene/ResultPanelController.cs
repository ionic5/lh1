using LikeLion.LH1.Client.Core.View.GameScene;
using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class ResultPanelController
    {
        private readonly GameHost _omokHost;
        private readonly IResultPanel _panel;
        private readonly Action _showPickStonePanel;

        public ResultPanelController(GameHost omokHost, IResultPanel panel, Action showPickStonePanel)
        {
            _omokHost = omokHost;
            _panel = panel;

            _panel.RestartButtonClickedEvent += OnRestartButtonClickedEvent;
            _showPickStonePanel = showPickStonePanel;
        }

        public void OnRestartButtonClickedEvent(object sender, EventArgs args)
        {
            _panel.RestartButtonClickedEvent -= OnRestartButtonClickedEvent;
            _panel.Hide();

            _omokHost.Reset();

            _showPickStonePanel.Invoke();
        }
    }
}
