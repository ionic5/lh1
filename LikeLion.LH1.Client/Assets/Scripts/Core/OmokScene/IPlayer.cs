using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public interface IPlayer
    {
        void HaltTurn();
        bool IsStoneOwner(int stoneType);
        void StartTurn();
    }
}
