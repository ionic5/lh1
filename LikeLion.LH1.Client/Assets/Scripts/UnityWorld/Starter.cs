using System;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld
{
    public class Starter : MonoBehaviour
    {
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

            Destroy(gameObject);
        }
    }
}
