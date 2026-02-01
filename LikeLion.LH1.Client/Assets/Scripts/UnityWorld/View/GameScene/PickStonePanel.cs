using LikeLion.LH1.Client.Core.View.GameScene;
using System;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld.View.GameScene
{
    public class PickStonePanel : MonoBehaviour, IPickStonePanel
    {
        public event EventHandler WhiteStoneButtonClickedEvent;
        public event EventHandler BlackStoneButtonClickedEvent;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnWhiteStoneButtonClicked()
        {
            WhiteStoneButtonClickedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void OnBlackStoneButtonClicked()
        {
            BlackStoneButtonClickedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
