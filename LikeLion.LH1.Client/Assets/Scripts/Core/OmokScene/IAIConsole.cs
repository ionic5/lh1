using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core
{
    public interface IAIConsole
    {
        Task<Tuple<int, int>> RequestStonePoint(int stoneType, int[][] array, System.Threading.CancellationToken token);
    }
}
