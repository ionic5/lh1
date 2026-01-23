namespace LikeLion.LH1.Client.Core.OmokScene
{
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
            // 다음 수를 비동기로 가져온다.
        }

        public void HaltTurn()
        {
        }
    }
}
