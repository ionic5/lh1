namespace LikeLion.LH1.Client.Core.OmokScene
{
    // TODO Use gemini ai.
    public class AIPlayer : IPlayer
    {
        private readonly Checkerboard _board;

        public AIPlayer(Checkerboard board)
        {
            _board = board;
        }

        public bool IsStoneOwner(int stoneType)
        {
            return StoneType.White == stoneType;
        }

        public void StartTurn()
        {
            _board.StonePointClickedEvent += OnStonePointClickedEvent;
        }

        public void HaltTurn()
        {
            _board.StonePointClickedEvent -= OnStonePointClickedEvent;
        }

        public void OnStonePointClickedEvent(object sender, StonePointClickedEventArgs args)
        {
            _board.PutStone(args.Column, args.Row, StoneType.White);
        }
    }
}
