namespace LikeLion.LH1.Client.Core.GameScene
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
