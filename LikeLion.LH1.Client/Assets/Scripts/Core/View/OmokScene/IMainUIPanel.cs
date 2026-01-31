namespace LikeLion.LH1.Client.Core.View.OmokScene
{
    public interface IMainUIPanel
    {
        void SetMainPlayerStone(int stoneType);
        void PlayTurnStartAnimation(int stoneType);
        void SetRemainTime(float remainTime);
        void SetCurrentPlayerStone(int stoneType);
    }
}
