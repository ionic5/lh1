using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.GameScene
{
    public class PlayerTurnFinishedEventArgs : EventArgs
    {
        public string PlayerGuid;
        public int StoneType;
        public int Column;
        public int Row;
    }
}
