using LikeLion.LH1.Client.Core.View.OmokScene;
using System;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld.View.OmokScene
{
    public class ResultPanel : MonoBehaviour, IResultPanel
    {
        [SerializeField]
        private GameObject _winPanel;
        [SerializeField]
        private GameObject _losePanel;

        public event EventHandler RestartButtonClickedEvent;

        public void SetResult(bool isWin)
        {
            _winPanel.SetActive(isWin);
            _losePanel.SetActive(!isWin);
        }

        public void OnRestartButtonClicked()
        {
            RestartButtonClickedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
