using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace ProductMadness.Gameplay
{
    public class WheelView : MonoBehaviour
    {
        #region Serialized Fields
        
        [SerializeField] private Transform wheelCircle ;

        [Space] 
        [SerializeField] private WheelConfiguration configuration;

        #endregion

        #region Events
        
        private UnityAction onSpinStartEvent;
        private UnityAction<WheelItem> onSpinEndEvent;

        #endregion

        #region Private Fields
        private WheelController controller;
        private bool isSpinning = false;
        #endregion
        
        private void Start ()
        {
            controller = new WheelController(configuration, this);
        }

        #region Public Methods

        /// <summary>
        /// Checks if we can spin with the give multiplier and then spins it
        /// </summary>
        /// <param name="multiplier"></param>
        /// <returns>Success spin</returns>
        public bool TrySpin (int multiplier)
        {
            if (configuration.WheelItems.All(item => item.Multiplier != multiplier)) return false;
            
            if (!isSpinning) {
                isSpinning = true ;
                if (onSpinStartEvent != null)
                    onSpinStartEvent.Invoke () ;

                controller.Spin(multiplier);
            }

            return true;
        }

        /// <summary>
        /// Rotates the wheel UI to the selected item 
        /// </summary>
        /// <param name="targetRotation"></param>
        /// <param name="halfPieceAngle"></param>
        /// <param name="item"></param>
        public void StartRotation(Vector3 targetRotation, float halfPieceAngle, WheelItem item)
        {
            float previousAngle, currentAngle;
            previousAngle = currentAngle = wheelCircle.eulerAngles.z;
            bool isIndicatorOnTheLine = false;
            
            wheelCircle
                .DORotate (targetRotation, configuration.SpinDuration, RotateMode.Fast)
                .SetEase (Ease.InOutQuart)
                .OnUpdate (() => {
                    float diff = Mathf.Abs (previousAngle - currentAngle);
                    if (diff >= halfPieceAngle) {
                        previousAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }
                    currentAngle = wheelCircle.eulerAngles.z;
                })
                .OnComplete (() => {
                    isSpinning = false;
                    if (onSpinEndEvent != null)
                        onSpinEndEvent.Invoke (item);

                    onSpinStartEvent = null; 
                    onSpinEndEvent = null;
                }) ;
        }
        
        public void OnSpinStart (UnityAction action) {
            onSpinStartEvent = action ;
        }

        public void OnSpinEnd (UnityAction<WheelItem> action) {
            onSpinEndEvent = action ;
        }

        #endregion
    }
}
