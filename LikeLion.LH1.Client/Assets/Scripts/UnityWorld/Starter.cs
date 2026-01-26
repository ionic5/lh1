using LikeLion.LH1.Client.Core.OmokScene;
using LikeLion.LH1.Client.UnityWorld.OmokScene;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld
{
    public class Starter : MonoBehaviour
    {
        [SerializeField]
        private View.OmokScene.Checkerboard _checkerboard;
        [SerializeField]
        private View.OmokScene.MainUIPanel _mainUIPanel;
        [SerializeField]
        private Loop _loop;

        private void Start()
        {
            var time = new Time();
            var board = new Core.OmokScene.Checkerboard(_checkerboard);
            var logger = new DebugLogger();

            var aiConsole = new AIConsole(logger);
            var aiPlayer = new AIPlayer(board, aiConsole);
            var mainPlayer = new MainPlayer(board);
            var players = new List<IPlayer>
            {
                mainPlayer,
                aiPlayer
            };

            var host = new OmokHost(board, players, new Core.Timer(time, _loop), 60, _mainUIPanel);
            _loop.Add(host);

            host.Start();
            host.GameFinishedEvent += (sender, args) =>
            {
                Debug.Log($"GameFinished. Win {args.WinnerStone}");
            };
        }
    }
}
