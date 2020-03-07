using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using SpaceOrigin.Data;

namespace SpaceOrigin.MahjongConnect
{
    public class WinPopUpWindow : PopUpWindow
    {
        public TextMeshProUGUI sessionScoreText;
        public TextMeshProUGUI highScoreText;
        public IntSO selectedLevelIndexSO;

        public override void ShowWindow()
        {
            base.ShowWindow();
            LevelStatus levelHistory = LevelHistory.GetLevelHistory(selectedLevelIndexSO.Value);
            if (levelHistory != null)
            {
                highScoreText.text = levelHistory.highScore.ToString();
                sessionScoreText.text = levelHistory.currentScore.ToString();
            }
        }

        protected override void OnCloseButtonClick()
        {
            SceneManager.LoadScene(Constants.titleSceenName, LoadSceneMode.Single);
        }
    }
}
