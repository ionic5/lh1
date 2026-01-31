using System;
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
        [SerializeField]
        private Screen _screen;

        private void Start()
        {
            var time = new Time();
            var logger = new DebugLogger();
            var assetLoader = new AssetLoader(logger);

            _screen.Logger = logger;
            _screen.AssetLoader = assetLoader;

            var gameSceneLoader = new GameSceneLoader(_screen, logger, time);
            var titleSceneLoader = new TitleSceneLoader(_screen, () => { gameSceneLoader.Load(); });

            titleSceneLoader.Load();
        }
    }
}
