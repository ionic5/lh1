using System.Threading;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class AIPlayer : Player, IPlayer
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

        public async void StartTurn()
        {
            _cts = new CancellationTokenSource();

            var point = await _aiConsole.RequestStonePoint(GetStoneType(), _board.ToArray(), _cts.Token);
            if (point == null)
                return;

            _board.PutStone(point.Item1, point.Item2, GetStoneType());
        }

        public void HaltTurn()
        {
            _cts?.Cancel();
        }
    }
}
