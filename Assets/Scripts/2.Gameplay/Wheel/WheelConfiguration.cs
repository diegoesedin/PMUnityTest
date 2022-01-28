using UnityEngine;

namespace ProductMadness.Gameplay
{
    [CreateAssetMenu(fileName = "WheelConfig", menuName = "Configuration/Wheel Configuration")]
    public class WheelConfiguration : ScriptableObject
    {
        [Header ("Wheel Settings")]
        [Range (1, 20)] public int SpinDuration = 8 ;

        [Space]
        [Header ("Prizes")]
        public WheelItem[] WheelItems ;
    }
}
