namespace ProductMadness.Architecture
{
    public class BoardPresenter : IBoardPresenter
    {
        #region Private Fields
        private BoardElement[,] boardElements;
        private BoardView view;
        private Board board;
        #endregion
        
        public BoardPresenter(int _width, int _height, BoardView _view)
        {
            board = new Board(_width, _height);
            
            boardElements = new BoardElement[_width, _height];
            view = _view;
        }

        #region Public Methods
        
        /// <summary>
        /// Add element to the board
        /// </summary>
        /// <param name="boardElement"></param>
        public void AddElement(BoardElement boardElement)
        {
            boardElement.OnMineExplode += UncoverMines;
            boardElement.OnNoMine += UncoverAreaWithoutMines;
            boardElements[boardElement.X, boardElement.Y] = boardElement;
        }

        public bool IsThereMineAt(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < board.Width && y < board.Height)
                return boardElements[x, y].IsMine;
            return false;
        }

        /// <summary>
        /// Search for mines around an element
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Quantity of mines around</returns>
        public int GetAdjacentMines(int x, int y)
        {
            int count = 0;

            if (IsThereMineAt(x,   y+1)) ++count;      // top
            if (IsThereMineAt(x+1, y+1)) ++count;     // top-right
            if (IsThereMineAt(x+1, y  )) ++count;      // right
            if (IsThereMineAt(x+1, y-1)) ++count;     // bottom-right
            if (IsThereMineAt(x,   y-1)) ++count;      // bottom
            if (IsThereMineAt(x-1, y-1)) ++count;     // bottom-left
            if (IsThereMineAt(x-1, y  )) ++count;      // left
            if (IsThereMineAt(x-1, y+1)) ++count;     // top-left

            return count;
        }

        public void UncoverAreaWithoutMines(int x, int y)
        {
            FillUncover(x, y, new bool[board.Width, board.Height]);
            
            if (IsFinished())
                view.ShowWin();
        }

        public void UncoverMines()
        {
            foreach (BoardElement boardElement in boardElements)
            {
                if (boardElement.IsMine) view.UncoverElement(boardElement.X, boardElement.Y, true, 0);
            }
            view.ShowLose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Classic flood fill algorithm to discover clear elements
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="visited"></param>
        private void FillUncover(int x, int y, bool[,] visited)
        {
            if (x >= 0 && y >= 0 && x < board.Width && y < board.Height) {
                // Check if we already visited this element
                if (visited[x, y])
                    return;

                int adjacents = GetAdjacentMines(x, y);
                // Discover cleaned element
                boardElements[x,y].Show();
                view.UncoverElement(x, y, false, adjacents);

                // If there is some mine we have to stop
                if (adjacents > 0)
                    return;

                visited[x, y] = true;

                // Recursion to clear adjacents
                FillUncover(x-1, y, visited);
                FillUncover(x+1, y, visited);
                FillUncover(x, y-1, visited);
                FillUncover(x, y+1, visited);
            }
        }

        private bool IsFinished()
        {
            foreach (BoardElement boardElement in boardElements)
                if (boardElement.IsCovered && !boardElement.IsMine)
                    return false;
            
            return true;
        }

        #endregion
    }
}
