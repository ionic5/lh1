using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld.View.OmokScene
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _winPanel;
        [SerializeField]
        private GameObject _losePanel;

        public void SetResult(bool isWin)
        {
            _winPanel.SetActive(isWin);
            _losePanel.SetActive(!isWin);
        }
    }
}
