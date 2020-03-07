using System;
using UnityEngine;

namespace SpaceOrigin
{
    public class LevelHistory 
    {
        public static LevelStatus GetLevelHistory(int levelIndex)
        {
            string key = levelIndex.ToString();
            if (PlayerPrefs.HasKey(key))
            {
                string jsonString = PlayerPrefs.GetString(key);
                LevelStatus levelStatus = JsonUtility.FromJson<LevelStatus>(jsonString);
                return levelStatus;
            }
            else
            {
               // Debug.Log("no pref exit key " + levelIndex);
            }
            return null;
        }

        public static void SaveLevelStatus(LevelStatus levelStatus)
        {
            string jsonString = JsonUtility.ToJson(levelStatus);
            string key = levelStatus.levelIndex.ToString();
            PlayerPrefs.SetString(key, jsonString);
            Debug.Log("saving prefs key " + levelStatus.levelIndex);
        }
    }

    [Serializable]
    public class LevelStatus
    {
        public int levelIndex;
        public bool completed;
        public int highScore;
        public int currentScore;
    }

}
