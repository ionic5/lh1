using LikeLion.LH1.Client.Core.OmokScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld.View.OmokScene
{
    public class StonePanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _whiteStone;
        [SerializeField]
        private GameObject _blackStone;

        public void SetStone(int stoneType)
        {
            _whiteStone.SetActive(stoneType == StoneType.White);
            _blackStone.SetActive(stoneType == StoneType.Black);
        }
    }
}
