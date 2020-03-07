using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceOrigin.MahjongConnect
{
    public class PopUpWindow : MonoBehaviour
    {
        public Button closeButton;
        public Animator windowAnimator;
        public GameObject windowRootObject;
       
        protected virtual void Awake()
        {
            windowRootObject.SetActive(false);
        }

        public virtual void ShowWindow()
        {
            windowRootObject.transform.localScale = Vector3.zero;
            windowRootObject.SetActive(true);
            windowAnimator.SetTrigger("ShowPopUp");
        }

        private void OnEnable()
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        protected virtual void OnCloseButtonClick()
        {
        }
    }
}
