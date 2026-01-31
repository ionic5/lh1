using LikeLion.LH1.Client.Core;
using LikeLion.LH1.Client.Core.View.Scenes;
using System;
using System.Collections;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld.View.Scenes
{
    public class TitleScene : Scene, ITitleScene
    {
        public event EventHandler OmokButtonClickedEvent;

        public void OnOmokButtonClicked()
        {
            OmokButtonClickedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}