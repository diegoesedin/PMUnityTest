using System;

namespace ProductMadness.Architecture
{
    public class BoardElement
    {
        public Action OnMineExplode;
        public Action<int, int> OnNoMine;
        
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsMine { get; private set; }
        public bool IsCovered { get; private set; }

        public BoardElement(int x, int y)
        {
            X = x;
            Y = y;
            IsMine = UnityEngine.Random.value < .15f;
            IsCovered = true;
        }

        public void Discover()
        {
            Show();
            if (IsMine)
                OnMineExplode?.Invoke();
            else
                OnNoMine?.Invoke(X, Y);
        }

        public void Show()
        {
            IsCovered = false;
        }
    }
}
