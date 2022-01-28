namespace ProductMadness.Architecture
{
    public interface IBoardView
    {
        void ShowWin();
        void ShowLose();
        void UncoverElement(int x, int y, bool isMine, int adjacents);
    }
}
