namespace ProductMadness.Architecture
{
    public class BoardElementPresenter : IBoardElementPresenter
    {
        #region Private Fields
        
        private BoardElement boardElement;
        private BoardElementView view;

        #endregion

        public BoardElementPresenter(BoardElement _boardElement, BoardElementView _view)
        {
            this.boardElement = _boardElement;
            view = _view;
        }

        public void OnElementClicked()
        {
            boardElement.Discover();
        }
    }
}
