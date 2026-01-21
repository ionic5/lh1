using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class OmokHost
    {
        public event EventHandler<GameFinishedEventArgs> GameFinishedEvent;
        private Checkerboard _checkerboard;
        private List<IPlayer> _players;

        public void Start()
        {
            var player = _players.First(entry => entry.IsStoneOwner(StoneType.Black));
            player.StartTurn();
        }

        private void OnStonePuttedEvent(object sender, StonePuttedEventArgs args)
        {
            var winnerStone = CheckWinner(_checkerboard.ToArray());
            if (winnerStone == StoneType.Null)
            {
                var player = _players.First(entry => entry.IsStoneOwner(args.StoneType));
                player.StartTurn();
                return;
            }

            GameFinishedEvent?.Invoke(this, new GameFinishedEventArgs { WinnerStone = winnerStone });
        }

        private int CheckWinner(int[][] board)
        {
            int rows = board.Length;
            if (rows == 0) return StoneType.Null;
            int cols = board[0].Length;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (board[r][c] == 0) continue;

                    int stone = board[r][c];

                    if (CheckDirection(board, r, c, 0, 1, stone) || // Horizontal
                        CheckDirection(board, r, c, 1, 0, stone) || // Vertical
                        CheckDirection(board, r, c, 1, 1, stone) || // Right down
                        CheckDirection(board, r, c, 1, -1, stone))  // Left down
                    {
                        return stone;
                    }
                }
            }

            return StoneType.Null;
        }

        private bool CheckDirection(int[][] board, int r, int c, int dr, int dc, int stone)
        {
            int count = 1;
            int rows = board.Length;
            int cols = board[0].Length;

            for (int i = 1; i < 5; i++)
            {
                int nr = r + (dr * i);
                int nc = c + (dc * i);

                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && board[nr][nc] == stone)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count == 5;
        }
    }
}
