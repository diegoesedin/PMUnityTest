using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductMadness.Architecture
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class BoardElementView : MonoBehaviour, IBoardElementView
    {
        #region Serialized Fields
        
        [SerializeField] private TextMeshProUGUI mineText;
        [SerializeField] private Sprite emptySprite, mineSprite;
        [SerializeField] private BoardElementConfiguration config;

        #endregion

        #region Private Fields
        
        private BoardElementPresenter boardElementPresenter;
        private Button button;
        private Image mineImage;

        #endregion

        #region Public Methods
        
        public void Init(BoardElement boardElement)
        {
            boardElementPresenter = new BoardElementPresenter(boardElement, this);
            mineImage = GetComponent<Image>();
            button = GetComponent<Button>();
            button.onClick.AddListener(OnMineClicked);
            
            mineText.SetText("");
        }
        

        public void SetSprite(bool isMine, int adjacentAmount)
        {
            if (!isMine)
            {
                mineImage.sprite = emptySprite;
                if (adjacentAmount > 0)
                {
                    mineText.SetText($"{adjacentAmount}");
                    mineText.color = config.ColorAdjacents[adjacentAmount - 1];
                }
            }
            else
                mineImage.sprite = mineSprite;
        }

        #endregion

        #region Private Methods
        private void OnMineClicked()
        {
            boardElementPresenter.OnElementClicked();
        }
        #endregion
    }
}
