using LikeLion.LH1.Client.Core.View.GameScene;
using System;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class PickStonePanelController
    {
        private readonly IPlayer _mainPlayer;
        private readonly IPickStonePanel _pickStonePanel;
        private readonly IGameSession _gameSession;

        public PickStonePanelController(IPlayer mainPlayer, IPickStonePanel pickStonePanel, IGameSession gameSession)
        {
            _mainPlayer = mainPlayer;
            _pickStonePanel = pickStonePanel;
            _gameSession = gameSession;

            _pickStonePanel.BlackStoneButtonClickedEvent += OnBlackStoneButtonClickedEvent;
            _pickStonePanel.WhiteStoneButtonClickedEvent += OnWhiteStoneButtonClickedEvent;
            _pickStonePanel.DestroyEvent += OnDestroyPanelEvent;

            _gameSession.GamePreparedEvent += OnGamePreparedEvent;
            _gameSession = gameSession;
        }

        private void OnGamePreparedEvent(object sender, EventArgs e)
        {
            DetachEventHandlers();

            _pickStonePanel.Hide();
        }

        private void OnDestroyPanelEvent(object sender, DestroyEventArgs e)
        {
            DetachEventHandlers();
        }

        private void DetachEventHandlers()
        {
            _pickStonePanel.BlackStoneButtonClickedEvent -= OnBlackStoneButtonClickedEvent;
            _pickStonePanel.WhiteStoneButtonClickedEvent -= OnWhiteStoneButtonClickedEvent;
            _pickStonePanel.DestroyEvent -= OnDestroyPanelEvent;

            _gameSession.GamePreparedEvent -= OnGamePreparedEvent;
        }

        public void OnWhiteStoneButtonClickedEvent(object sender, EventArgs args)
        {
            PickStone(StoneType.White);
        }

        public void OnBlackStoneButtonClickedEvent(object sender, EventArgs args)
        {
            PickStone(StoneType.Black);
        }

        private void PickStone(int stoneType)
        {
            _gameSession.PickStone(_mainPlayer.GetGameGuid(), _mainPlayer.GetPlayerGuid(), stoneType);
        }
    }
}
