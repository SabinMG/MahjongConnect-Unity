using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SpaceOrigin.MahjongConnect
{
    public class GameUIController : MonoBehaviour
    {
        public GameManager gameManger; // direct referencing
        public TextMeshProUGUI scoreText;
        public WinPopUpWindow winPopUpWindow;
        public LosePopUpWindow losePopUpWindow;
        public Button gameQuitButton;

        private void OnEnable()
        {
            gameQuitButton.onClick.AddListener(OnGameLost);
            gameManger.ongameScoreUpdate += OnGameScoreUpdate;
            gameManger.onGameWin += OnGameWin;
            gameManger.onGameLost += OnGameLost;
        }

        private void OnDisable()
        {
            gameQuitButton.onClick.RemoveListener(OnGameLost);
            gameManger.ongameScoreUpdate -= OnGameScoreUpdate;
            gameManger.onGameWin -= OnGameWin;
            gameManger.onGameLost -= OnGameLost;
        }

        private void OnGameScoreUpdate(int newScore)
        {
            scoreText.text = newScore.ToString();
        }

        private void OnGameWin()
        {
            gameQuitButton.gameObject.SetActive(false);
            winPopUpWindow.ShowWindow();
        }

        private void OnGameLost()
        {
            gameQuitButton.gameObject.SetActive(false);
            losePopUpWindow.ShowWindow();
        }
    }
}
