namespace LikeLion.LH1.Client.Core.GameScene
{
    public interface IPlayer : IDestroyable
    {
        void SetPlayerGuid(string playerGuid);
        string GetPlayerGuid();
        int GetStoneType();
        void HaltTurn();
        bool IsStoneOwner(int stoneType);
        void SetStone(int white);
        void StartTurn();
    }
}
