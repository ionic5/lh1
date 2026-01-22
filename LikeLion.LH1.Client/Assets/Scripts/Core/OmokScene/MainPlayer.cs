using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.Core.OmokScene
{
    public class MainPlayer : IPlayer
    {
        private readonly Checkerboard _board;

        public MainPlayer(Checkerboard board)
        {
            _board = board;
        }

        public bool IsStoneOwner(int stoneType)
        {
            return StoneType.Black == stoneType;
        }

        public void StartTurn()
        {
            _board.StonePointClickedEvent += OnStonePointClickedEvent;
        }

        public void HaltTurn()
        {
            _board.StonePointClickedEvent -= OnStonePointClickedEvent;
        }

        public void OnStonePointClickedEvent(object sender, StonePointClickedEventArgs args)
        {
            _board.PutStone(args.Column, args.Row, StoneType.Black);
        }
    }
}
