using Assets.Scripts.UnityWorld.View.Scenes;
using LikeLion.LH1.Client.Core;
using LikeLion.LH1.Client.Core.GameScene;
using LikeLion.LH1.Client.Core.View.GameScene;
using LikeLion.LH1.Client.UnityWorld.GameScene;
using System;
using System.Collections.Generic;

namespace LikeLion.LH1.Client.UnityWorld
{
    public class GameSceneLoader
    {
        private readonly Screen _screen;
        private readonly Core.ILogger _logger;
        private readonly ITime _time;
        private readonly Action _loadTitleScene;

        public GameSceneLoader(Screen screen, ILogger logger, ITime time, Action loadTitleScene)
        {
            _screen = screen;
            _logger = logger;
            _time = time;
            _loadTitleScene = loadTitleScene;
        }

        public async void Load()
        {
            await _screen.ShowLoadingBlind();
            _screen.DestroyLastScene();

            var instance = await _screen.AttachNewScene("Assets/Addressables/GameScene.prefab");
            var scene = instance.GetComponent<Assets.Scripts.UnityWorld.View.Scenes.GameScene>();

            var checkerBoard = scene.CheckerBoard;
            var loop = scene.Loop;
            var mainUIPanel = scene.MainUIPanel;
            var panelStack = scene.PanelStack;

            var board = new Core.GameScene.Checkerboard(checkerBoard, _logger);

            var aiConsole = new AIConsole(_logger);
            var aiPlayer = new AIPlayer(board, aiConsole);
            var mainPlayer = new MainPlayer(board);
            var players = new List<IPlayer>
            {
                mainPlayer,
                aiPlayer
            };

            var host = new GameHost(board, players, new Core.Timer(_time, loop), 5, mainUIPanel);

            Action showPickStonePanel = () =>
            {
                IPickStonePanel pickStonePanel = panelStack.ShowPickStonePanel();
                var ctrl = new PickStonePanelController(mainPlayer, aiPlayer, host, pickStonePanel);
            };

            host.StartGameEvent += (sender, args) =>
            {
                mainUIPanel.Show();
                mainUIPanel.SetMainPlayerStone(mainPlayer.GetStoneType());
            };
            host.GameFinishedEvent += (sender, args) =>
            {
                mainUIPanel.Hide();

                var panel = panelStack.ShowResultPanel();
                panel.SetResult(mainPlayer.IsStoneOwner(args.WinnerStone));
                var ctrl = new ResultPanelController(host, panel, showPickStonePanel, _loadTitleScene);
            };
            loop.Add(host);

            showPickStonePanel.Invoke();

            _screen.HideLoadingBlind();
        }
    }
}