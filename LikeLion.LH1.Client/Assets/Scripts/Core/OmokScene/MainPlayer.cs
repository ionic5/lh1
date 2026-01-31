namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class MainPlayer : Player, IPlayer
    {
        private readonly Checkerboard _board;

        public MainPlayer(Checkerboard board)
        {
            _board = board;
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
            _board.PutStone(args.Column, args.Row, GetStoneType());
        }
    }
}
