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
            var scene = instance.GetComponent<View.Scenes.GameScene>();

            var checkerBoard = scene.CheckerBoard;
            var loop = scene.Loop;
            var mainUIPanel = scene.MainUIPanel;
            var panelStack = scene.PanelStack;

            //var board = new Core.GameScene.Checkerboard(checkerBoard, _logger);

            //var aiPlayer = new AIPlayer(board, new AIConsole(_logger), _logger);
            //var mainPlayer = new MainPlayer(board);
            //var players = new List<IPlayer>
            //{
            //    mainPlayer,
            //    aiPlayer
            //};

            //var host = new GameHost(board, players, new Core.Timer(_time, loop), 60, mainUIPanel);

            //Action showPickStonePanel = () =>
            //{
            //    IPickStonePanel pickStonePanel = panelStack.ShowPickStonePanel();
            //    var ctrl = new PickStonePanelController(mainPlayer, aiPlayer, host, pickStonePanel);
            //};
            //EventHandler startGameEvtHdlr = (sender, args) =>
            //{
            //    mainUIPanel.Show();
            //    mainUIPanel.SetMainPlayerStone(mainPlayer.GetStoneType());
            //};
            //EventHandler<GameFinishedEventArgs> gameFinishedEvtHdlr = (sender, args) =>
            //{
            //    mainUIPanel.Hide();

            //    var panel = panelStack.ShowResultPanel();
            //    panel.SetResult(mainPlayer.IsStoneOwner(args.WinnerStone));
            //    var ctrl = new ResultPanelController(host, panel, showPickStonePanel, _loadTitleScene);
            //};
            //host.StartGameEvent += startGameEvtHdlr;
            //host.GameFinishedEvent += gameFinishedEvtHdlr;
            //loop.Add(host);

            //EventHandler<DestroyEventArgs> destroySceneEvtHdlr = null;
            //destroySceneEvtHdlr = (sender, args) =>
            //{
            //    scene.DestroyEvent -= destroySceneEvtHdlr;

            //    host.StartGameEvent -= startGameEvtHdlr;
            //    host.GameFinishedEvent -= gameFinishedEvtHdlr;
            //    loop.Remove(host);
            //    host.Destroy();

            //    foreach (var entry in players)
            //        entry.Destroy();
            //    players.Clear();

            //    board.Destroy();
            //};
            //scene.DestroyEvent += destroySceneEvtHdlr;

            //var session = new MockGameSession();

            //EventHandler connectedHdlr = null;
            //connectedHdlr = (sender, args) =>
            //{
            //    session.ConnectedEvent -= connectedHdlr;
            //};
            //session.ConnectedEvent += connectedHdlr;

            //EventHandler gameStartedHdlr = null;
            //gameStartedHdlr = (sender, args) =>
            //{
            //    session.GameStartedEvent -= gameStartedHdlr;

            //    showPickStonePanel.Invoke();
            //};
            //session.GameStartedEvent += gameStartedHdlr;

            //session.RequestConnect();

            _screen.HideLoadingBlind();
        }
    }
}