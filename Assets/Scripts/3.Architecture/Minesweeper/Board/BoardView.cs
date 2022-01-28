using UnityEngine;
using UnityEngine.UI;

namespace ProductMadness.Architecture
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BoardView : MonoBehaviour, IBoardView
    {
        #region Serialized Fields
        
        [SerializeField] private int width, height;
        [SerializeField] private BoardElementView boardElementViewPrefab;

        #endregion

        #region Private Fields
        
        private GridLayoutGroup grid;
        private BoardPresenter boardPresenter;
        private BoardElementView[,] elements;

        #endregion

        private void Start()
        {
            boardPresenter = new BoardPresenter(width, height, this);
            elements = new BoardElementView[width, height];
            grid = GetComponent<GridLayoutGroup>();
            grid.constraintCount = width;

            // Board UI generation
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var elementGo = Instantiate(boardElementViewPrefab, transform);
                    BoardElement boardElement = new BoardElement(x, y);
                    elementGo.Init(boardElement);
                    elements[x, y] = elementGo;
                    boardPresenter.AddElement(boardElement);
                }
            }
        }

        #region Public Methods
        
        public void ShowWin()
        {
            // todo: Show some UI as win feedback
            Debug.Log("WIN");
        }

        public void ShowLose()
        {
            // todo: Show some UI as lose feedback
            Debug.Log("You lose");
        }

        /// <summary>
        /// Show an element of the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isMine"></param>
        /// <param name="adjacents"></param>
        public void UncoverElement(int x, int y, bool isMine, int adjacents)
        {
            elements[x, y].SetSprite(isMine, adjacents);
        }

        #endregion
    }
}
