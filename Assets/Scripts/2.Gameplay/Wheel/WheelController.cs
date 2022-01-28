using System.Linq;
using UnityEngine;

namespace ProductMadness.Gameplay
{
    public class WheelController
    {
        #region Private Fields
        
        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;
        private WheelConfiguration configuration;
        private WheelView view;

        #endregion
        
        public WheelController(WheelConfiguration _configuration, WheelView _view)
        {
            view = _view;
            configuration = _configuration;
            
            // First calculation to know each piece inside the wheel
            pieceAngle = 360 / configuration.WheelItems.Length;
            halfPieceAngle = pieceAngle / 2f ;
            halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);
        }

        #region Public Methods

        public void Spin(int multiplier)
        {
            // Search for a piece inside the wheel that has the multiplier given
            var wheelsForMultiplier = configuration.WheelItems.Where(item => item.Multiplier == multiplier).ToArray();
            WheelItem wheelItem = wheelsForMultiplier[Random.Range(0, wheelsForMultiplier.Length - 1)];

            // Found the angle of the selected item and add it an offset inside it
            float angle = -(pieceAngle * wheelItem.Index);
            float rightOffset = (angle - halfPieceAngleWithPaddings) % 360;
            float leftOffset = (angle + halfPieceAngleWithPaddings) % 360;
            float randomAngle = Random.Range (leftOffset, rightOffset);

            Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * configuration.SpinDuration);
            
            // Rotate the wheel UI to the selected item
            view.StartRotation(targetRotation, halfPieceAngle, wheelItem);
        }

        #endregion
    }
}