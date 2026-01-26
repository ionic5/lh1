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

        public void SetRemainTime(float remainTime)
        {
            _remainTimeText.text = ((int)remainTime).ToString();
        }
    }
}
