using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductMadness.Architecture
{
    [CreateAssetMenu(fileName = "BoardElementDefaultConfig", menuName = "Configuration/Minesweeper/Board Element")]
    public class BoardElementConfiguration : ScriptableObject
    {
        public Color[] ColorAdjacents;
    }
}
