using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceOrigin.MahjongConnect
{
    public class LosePopUpWindow : PopUpWindow
    {
        public override void ShowWindow()
        {
            base.ShowWindow(); 
        }

        protected override void OnCloseButtonClick()
        {
            SceneManager.LoadScene(Constants.titleSceenName, LoadSceneMode.Single);
        }
    }
}
