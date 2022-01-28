namespace ProductMadness.Architecture
{
    public interface IBoardPresenter
    {
        void AddElement(BoardElement boardElement);
        bool IsThereMineAt(int x, int y);
        int GetAdjacentMines(int x, int y);
        void UncoverAreaWithoutMines(int x, int y);
        void UncoverMines();
    }
}
