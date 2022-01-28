using System;
using ProductMadness.Gameplay.API;
using Server.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductMadness.Gameplay
{
    public class WheelGameView : MonoBehaviour
    {
        #region Serialized fields
        [Header("Results texts")]
        [SerializeField] private TextMeshProUGUI originalScoreText;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private TextMeshProUGUI finalScoreText;

        [Space] [Header("Navigation")] 
        [SerializeField] private GameObject loading;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button spinButton;
        [Space]
        [SerializeField] private WheelView wheel;
        #endregion

        #region Private fields
        private IGameplayApi gameplay;
        private int playerScore;
        #endregion

        #region Unity Methods
        private void Start()
        {
            spinButton.onClick.AddListener(RequestSpin);
            closeButton.onClick.AddListener(Close);
            
            // This should be encapsulated by another class in a bigger game
            gameplay = new GameplayController();
            gameplay.RequestInitialBalance(OnGetBalance, OnError);
            
            ShowView();
        }
        #endregion

        #region Public Methods

        // Public method that should be called at start of this view from the outside
        public void ShowView()
        {
            HideTexts();
        }

        #endregion

        #region Private Methods
        private void HideTexts()
        {
            originalScoreText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
            finalScoreText.gameObject.SetActive(false);
        }

        private void Close()
        {
            // Here we go back to the rest of the game
            Debug.Log("Close game.");
        }
        
        private void RequestSpin()
        {
            multiplierText.gameObject.SetActive(false);
            finalScoreText.gameObject.SetActive(false);
            spinButton.interactable = false;
            
            wheel.OnSpinEnd(OnWheelSpinEnds);
            
            // Wait for the multiplier sent by API to spin the wheel
            SetLoading(true);
            gameplay.RequestMultiplier(OnGetMultiplier, OnError);
        }

        /// <summary>
        /// Callback for initial player balance, it should be called before start game
        /// </summary>
        /// <param name="balance"></param>
        private void OnGetBalance(int balance)
        {
            playerScore = balance;
            originalScoreText.SetText($"{balance}");
            originalScoreText.gameObject.SetActive(true);
            
            // After get the balance we are ready to play the game
            SetLoading(false);
        }

        private void OnGetMultiplier(int multiply)
        {
            // Validate if we can spin with this multiplier and our configuration
            bool canSpin = wheel.TrySpin(multiply);
            if (!canSpin)
                throw new Exception($"Can't spin with this multiplier: {multiply}");
            
            SetLoading(false);
        }

        private void OnGetPlayerBalance(long balance)
        {
            Debug.Log($"Current player balance: {balance}. Adding new score: {playerScore}");
            long newBalance = balance + playerScore;
            
            gameplay.SetPlayerBalance(newBalance, () => Debug.Log($"[Gameplay API] New balance saved: {newBalance}"), OnError);
        }

        private void OnError(Exception exception)
        {
            Debug.LogError($"<color=Green>[Gameplay API]</color> Error on response: {exception.Message}");
        }

        private void OnWheelSpinEnds(WheelItem item)
        {
            multiplierText.gameObject.SetActive(true);
            multiplierText.SetText($"{item.Multiplier}");
            spinButton.interactable = true;
            
            // The spin finished and we are ready to calculate the new score
            playerScore *= item.Multiplier;
            
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.SetText($"{playerScore}");
            
            // Request the player balance to then update it with the new one
            gameplay.RequestPlayerBalance(OnGetPlayerBalance, OnError);
        }

        private void SetLoading(bool show)
        {
            loading.SetActive(show);
        }
        #endregion
    }
}
