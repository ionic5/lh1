using LikeLion.LH1.Client.Core.OmokScene;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld
{
    public class Starter : MonoBehaviour
    {
        [SerializeField]
        private View.OmokScene.Checkerboard _checkerboard;
        [SerializeField]
        private Loop _loop;

        private void Start()
        {
            var time = new Time();
            var board = new Core.OmokScene.Checkerboard(_checkerboard);

            var aiPlayer = new AIPlayer(board);
            var mainPlayer = new MainPlayer(board);
            var players = new List<IPlayer>
            {
                mainPlayer,
                aiPlayer
            };

            var host = new OmokHost(board, players, new Core.Timer(time, _loop), 60);

            host.Start();
            host.GameFinishedEvent += (sender, args) =>
            {
                Debug.Log($"GameFinished. Win {args.WinnerStone}");
            };
        }
    }
}
