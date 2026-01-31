namespace LikeLion.LH1.Client.Core.OmokScene
{
    public interface IPlayer
    {
        int GetStoneType();
        void HaltTurn();
        bool IsStoneOwner(int stoneType);
        void SetStone(int white);
        void StartTurn();
    }
}
