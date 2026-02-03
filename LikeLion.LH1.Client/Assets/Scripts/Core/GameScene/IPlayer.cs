namespace LikeLion.LH1.Client.Core.GameScene
{
    public interface IPlayer : IDestroyable
    {
        void SetPlayerGuid(string playerGuid);
        string GetPlayerGuid();
        int GetStoneType();
        void SetStone(int stoneType);
        bool IsStoneOwner(int stoneType);
        void StartTurn();
        void HaltTurn();
    }
}
