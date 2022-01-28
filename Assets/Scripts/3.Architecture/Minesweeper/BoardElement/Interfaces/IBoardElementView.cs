namespace ProductMadness.Architecture
{
    public interface IBoardElementView
    {
        void Init(BoardElement boardElement);
        void SetSprite(bool isMine, int adjacentAmount);
    }
}
