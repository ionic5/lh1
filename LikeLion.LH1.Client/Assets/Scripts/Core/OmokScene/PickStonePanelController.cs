using System;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class PickStonePanelController
    {
        private readonly IPlayer _opponentPlayer;
        private readonly IPlayer _mainPlayer;

        public PickStonePanelController(IPlayer mainPlayer, IPlayer opponentPlayer)
        {
            _mainPlayer = mainPlayer;
            _opponentPlayer = opponentPlayer;
        }

        public void OnWhiteStoneButtonClickedEvent(object sender, EventArgs args)
        {
            _mainPlayer.SetStone(StoneType.White);
            _opponentPlayer.SetStone(StoneType.Black);
        }

        public void OnBlackStoneButtonClickedEvent(object sender, EventArgs args)
        {
            _mainPlayer.SetStone(StoneType.Black);
            _opponentPlayer.SetStone(StoneType.White);
        }
    }
}
