using LikeLion.LH1.Client.Core.View.OmokScene;
using System;
using TMPro;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld.View.OmokScene
{
    public class MainUIPanel : MonoBehaviour, IMainUIPanel
    {
        [SerializeField]
        private TMP_Text _remainTimeText;

        [SerializeField]
        private StonePanel _currentPlayerStonePanel;
        [SerializeField]
        private StonePanel _mainPlayerStonePanel;
        [SerializeField]
        private TurnAlarmPanel _turnAlarmPanel;
        [SerializeField]
        private ResultPanel _resultPanel;

        public void SetRemainTime(float remainTime)
        {
            _remainTimeText.text = ((int)remainTime).ToString();
        }

        public void SetMainPlayerStone(int stoneType)
        {
            _mainPlayerStonePanel.SetStone(stoneType);
        }

        public void PlayTurnStartAnimation(int stoneType)
        {
            _turnAlarmPanel.Play(stoneType);
        }

        public void SetCurrentPlayerStone(int stoneType)
        {
            _currentPlayerStonePanel.SetStone(stoneType);
        }

        public void ShowResultPanel(bool isWin)
        {
            _resultPanel.gameObject.SetActive(true);
            _resultPanel.SetResult(isWin);
        }
    }
}
