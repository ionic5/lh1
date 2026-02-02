namespace LikeLion.LH1.Client.Core.GameScene
{
    public interface IPlayer : IDestroyable
    {
        string GetGameGuid();
        void SetGameGuid(string gameGuid);
        void SetPlayerGuid(string playerGuid);
        string GetPlayerGuid();
        int GetStoneType();
        void SetStone(int stoneType);
        bool IsStoneOwner(int stoneType);
        void StartTurn();
        void HaltTurn();
    }
}
