using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface IMainUIPanel
    {
        void SetMainPlayerStone(int stoneType);
        void PlayTurnStartAnimation(int stoneType);
        void SetRemainTime(float remainTime);
        void SetCurrentPlayerStone(int stoneType);
        void ShowResultPanel(bool isWin);
    }
}
