using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class MockGameSession : IGameSession
    {
        public event EventHandler<ConnectedEventArgs> ConnectedEvent;
        public event EventHandler<GameCreatedEventArgs> GameCreatedEvent;
        public event EventHandler<GamePreparedEventArgs> GamePreparedEvent;
        public event EventHandler<PlayerTurnStartedEventArgs> PlayerTurnStartedEvent;
        public event EventHandler<PlayerTurnFinishedEventArgs> PlayerTurnFinishedEvent;
        public event EventHandler<GameFinishedEventArgs> GameFinishedEvent;

        private List<Player> _players;
        private IAIConsole _aiConsole;
        private Core.Timer _timer;
        private string _gameGuid;
        private List<List<int>> _board;

        private class Player
        {
            public string PlayerGuid;
            public int StoneType;
        }

        public MockGameSession(IAIConsole aiConsole, Timer timer)
        {
            _players = new List<Player>();
            _aiConsole = aiConsole;
            _timer = timer;

            _board = new List<List<int>>();
            for (int i = 0; i < 19; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < 19; j++)
                    row.Add(StoneType.Null);
                _board.Add(row);
            }
        }

        public void PickStone(string gameGuid, string playerGuid, int stoneType)
        {
            var player = _players.First(entry => entry.PlayerGuid == playerGuid);
            player.StoneType = stoneType;

            var otherPlayer = _players.First(entry => entry.PlayerGuid != playerGuid);
            otherPlayer.StoneType = StoneType.Black == stoneType ? StoneType.White : StoneType.Black;

            var playerStones = new List<PlayerStone>
            {
                new PlayerStone { PlayerGuid = player.PlayerGuid, StoneType = player.StoneType },
                new PlayerStone { PlayerGuid = otherPlayer.PlayerGuid, StoneType = otherPlayer.StoneType }
            };
            GamePreparedEvent?.Invoke(this, new GamePreparedEventArgs { PlayerStones = playerStones });
        }

        public void PutStone(string gameGuid, string playerGuid, int column, int row)
        {
            var stoneType = _players.Where(entry => entry.PlayerGuid == playerGuid).Select(entry => entry.StoneType).First();

            _board[column][row] = stoneType;

            _timer.Stop(0);

            PlayerTurnFinishedEvent?.Invoke(this, new PlayerTurnFinishedEventArgs
            {
                PlayerGuid = playerGuid,
                StoneType = stoneType,
                Column = column,
                Row = row
            });

            var winnerStone = CheckWinner(_board.Select(row => row.ToArray()).ToArray());
            if (winnerStone == StoneType.Null)
            {
                var otherPlayerGuid = _players.Where(entry => entry.PlayerGuid != playerGuid).Select(entry => entry.PlayerGuid).First();
                StartTurn(otherPlayerGuid);
            }
            else
            {
                PlayerTurnStartedEvent -= OnPlayerTurnStartedEvent;

                var winner = _players.Where(entry => entry.StoneType == winnerStone).First();
                GameFinishedEvent?.Invoke(this, new GameFinishedEventArgs
                {
                    WinnerGuid = winner.PlayerGuid,
                    WinnerStone = winner.StoneType
                });
            }
        }

        public void RequestConnect()
        {
            var mainPlayer = new Player { PlayerGuid = Guid.NewGuid().ToString(), StoneType = StoneType.Null };
            _players.Add(mainPlayer);

            ConnectedEvent?.Invoke(this, new ConnectedEventArgs { PlayerGuid = mainPlayer.PlayerGuid });
        }

        public void RequestGame(string playerGuid)
        {
            _players.Add(new Player { PlayerGuid = Guid.NewGuid().ToString(), StoneType = StoneType.Null });

            GameCreatedEvent?.Invoke(this, new GameCreatedEventArgs { GameGuid = Guid.NewGuid().ToString() });
        }

        public void StartGame(string gameGuid, string playerGuid)
        {
            var player = _players.First(entry => entry.StoneType == StoneType.Black);

            PlayerTurnStartedEvent += OnPlayerTurnStartedEvent;

            StartTurn(player.PlayerGuid);
        }

        private void StartTurn(string playerGuid)
        {
            PlayerTurnStartedEvent?.Invoke(this, new PlayerTurnStartedEventArgs { PlayerGuid = playerGuid });

            _timer.Start(0, 60, () =>
            {
                PlayerTurnFinishedEvent?.Invoke(this, new PlayerTurnFinishedEventArgs
                {
                    PlayerGuid = playerGuid,
                    StoneType = StoneType.Null,
                    Column = -1,
                    Row = -1
                });

                var otherPlayerGuid = _players.Where(entry => entry.PlayerGuid != playerGuid).Select(entry => entry.PlayerGuid).First();
                StartTurn(otherPlayerGuid);
            });
        }

        private async void OnPlayerTurnStartedEvent(object sender, PlayerTurnStartedEventArgs args)
        {
            var player = _players[1];
            if (player.PlayerGuid != args.PlayerGuid)
                return;

            var cts = new CancellationTokenSource();
            _timer.Start(1, 60, () =>
            {
                cts?.Cancel();
                cts?.Dispose();
            });

            var point = await _aiConsole.RequestStonePoint(player.StoneType, _board.Select(row => row.ToArray()).ToArray(), cts.Token);

            _timer.Stop(1);
            if (point == null)
                return;
            PutStone(_gameGuid, player.PlayerGuid, point.Item1, point.Item2);
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
