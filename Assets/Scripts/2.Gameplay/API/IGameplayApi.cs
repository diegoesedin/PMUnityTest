using System;

namespace ProductMadness.Gameplay.API
{
    public interface IGameplayApi
    {
        void RequestPlayerBalance(Action<long> OnResponse, Action<Exception> OnError);
        void RequestInitialBalance(Action<int> OnResponse, Action<Exception> OnError);
        void RequestMultiplier(Action<int> OnResponse, Action<Exception> OnError);
        void SetPlayerBalance(long newBalance, Action OnResponse, Action<Exception> OnError);
    }
}
