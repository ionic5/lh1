using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class Player
    {
        private int _stoneType;

        public bool IsStoneOwner(int stoneType)
        {
            return _stoneType == stoneType;
        }

        public int GetStoneType()
        {
            return _stoneType;
        }

        public void SetStone(int stoneType)
        {
            _stoneType = stoneType;
        }
    }
}
