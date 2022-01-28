using System;
using Server.API;

namespace ProductMadness.Gameplay.API
{
    /// <summary>
    ///  Note: this can be replaced by another implementation, for example with http requests
    /// </summary>
    public class GameplayController : IGameplayApi
    {
        private GameplayApi gameplayApi;
        
        public GameplayController()
        {
            gameplayApi = new GameplayApi();
            gameplayApi.Initialise();
        }

        public void RequestPlayerBalance(Action<long> OnResponse, Action<Exception> OnError)
        {
            gameplayApi.GetPlayerBalance().Then(OnResponse, OnError).Catch(OnError);
        }
        
        public void RequestInitialBalance(Action<int> OnResponse, Action<Exception> OnError)
        {
            gameplayApi.GetInitialWin().Then(OnResponse, OnError).Catch(OnError);
        }
        
        public void RequestMultiplier(Action<int> OnResponse, Action<Exception> OnError)
        {
            gameplayApi.GetMultiplier().Then(OnResponse, OnError).Catch(exception =>
            {
                RequestMultiplier(OnResponse, OnError);
            });
        }
        
        public void SetPlayerBalance(long newBalance, Action OnResponse, Action<Exception> OnError)
        {
            gameplayApi.SetPlayerBalance(newBalance).Then(OnResponse, OnError).Catch(OnError);
        }
    }
}
