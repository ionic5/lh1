using LikeLion.LH1.Client.Core.OmokScene;
using LikeLion.LH1.Client.Core.View.OmokScene;
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
        private View.OmokScene.PanelStack _panelStack;
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
            host.StartGameEvent += (sender, args) =>
            {
                _mainUIPanel.Show();
                _mainUIPanel.SetMainPlayerStone(mainPlayer.GetStoneType());
            };
            host.GameFinishedEvent += (sender, args) =>
            {
                _mainUIPanel.Hide();

                var panel = _panelStack.ShowResultPanel();
                panel.SetResult(mainPlayer.IsStoneOwner(args.WinnerStone));
                var ctrl = new ResultPanelController(host, panel);
            };
            _loop.Add(host);

            IPickStonePanel pickStonePanel = _panelStack.ShowPickStonePanel();
            var ctrl = new PickStonePanelController(mainPlayer, aiPlayer, host, pickStonePanel);
        }
    }
}
