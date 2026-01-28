using System.Threading;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class AIPlayer : IPlayer
    {
        private readonly Checkerboard _board;
        private readonly IAIConsole _aiConsole;
        private CancellationTokenSource _cts;

        public AIPlayer(Checkerboard board, IAIConsole aiConsole)
        {
            _board = board;
            _aiConsole = aiConsole;
            _cts = null;
        }

        public bool IsStoneOwner(int stoneType)
        {
            return StoneType.White == stoneType;
        }

        public async void StartTurn()
        {
            _cts = new CancellationTokenSource();

            var point = await _aiConsole.RequestStonePoint(StoneType.White, _board.ToArray(), _cts.Token);
            _board.PutStone(point.Item1, point.Item2, StoneType.White);
        }

        public void HaltTurn()
        {
            _cts?.Cancel();
        }

        public int GetStoneType()
        {
            return StoneType.White;
        }
    }
}
