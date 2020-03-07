using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceOrigin.Data;
using UnityEngine.SceneManagement;

namespace SpaceOrigin.MahjongConnect
{
    public class TitleUIController : MonoBehaviour
    {
        public LevelSelectButton[] levelSelectButtons;
        public IntSO selectedLevelIndexSO;
        public AudioSource audioSource;

        public bool deleteAllPlayerPrefs; // for testing puropose

        void Start()
        {
            if (deleteAllPlayerPrefs)
            {
               PlayerPrefs.DeleteAll();
            }

            for (int i = 0; i < levelSelectButtons.Length; i++)
            {
                LevelStatus levelHistory = LevelHistory.GetLevelHistory(i);
                if (levelHistory != null)
                {
                    bool levelCompleted = levelHistory.completed;
                    if (levelCompleted) levelSelectButtons[i].SetStar();
                }
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < levelSelectButtons.Length; i++)
            {
                levelSelectButtons[i].onButtonClick += OnLevelSelectBtnPress;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < levelSelectButtons.Length; i++)
            {
                levelSelectButtons[i].onButtonClick -= OnLevelSelectBtnPress;
            }
        }

        private void OnLevelSelectBtnPress(int levelIndex)
        {
            audioSource.Play();
            selectedLevelIndexSO.Value = levelIndex;
            SceneManager.LoadScene(Constants.gameSceenName, LoadSceneMode.Single);
        }
    }
}
