using LikeLion.LH1.Client.Core.View.GameScene;
using System;
using System.Linq;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class PickStonePanelController
    {
        private readonly IPickStonePanel _pickStonePanel;
        private readonly IGameSession _gameSession;
        private readonly string _gameGuid;
        private readonly IPlayer _player;
        private readonly Action _startGame;

        public PickStonePanelController(string gameGuid, IPlayer player,
            IPickStonePanel pickStonePanel, IGameSession gameSession, Action startGame)
        {
            _gameGuid = gameGuid;
            _gameSession = gameSession;
            _player = player;
            _pickStonePanel = pickStonePanel;
            _startGame = startGame;

            _pickStonePanel.BlackStoneButtonClickedEvent += OnBlackStoneButtonClickedEvent;
            _pickStonePanel.WhiteStoneButtonClickedEvent += OnWhiteStoneButtonClickedEvent;
            _pickStonePanel.DestroyEvent += OnDestroyPanelEvent;

            _gameSession.GamePreparedEvent += OnGamePreparedEvent;
        }

        private void OnGamePreparedEvent(object sender, GamePreparedEventArgs args)
        {
            DetachEventHandlers();
            _pickStonePanel.Hide();

            var stoneType = args.PlayerStones.Where(entry => entry.PlayerGuid == _player.GetPlayerGuid()).Select(entry => entry.StoneType).First();
            _player.SetStone(stoneType);
            _startGame.Invoke();
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
            _gameSession.PickStone(_gameGuid, _player.GetPlayerGuid(), stoneType);
        }
    }
}
