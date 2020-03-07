using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceOrigin.MahjongConnect
{
    [RequireComponent(typeof(Button))]
    public class LevelSelectButton : MonoBehaviour
    {
        public int levelIndex;
        public Image starImage;
  
        private Button _selectButon;

        public delegate void LevelSelectButtonDelegate(int levelIndex);
        public LevelSelectButtonDelegate onButtonClick;

        private void Awake()
        {
            starImage.enabled = false;
            _selectButon = GetComponent<Button>();
        }

        private void Start()
        {
        }

        void Update()
        {
        }

        private void OnEnable()
        {
            _selectButon.onClick.AddListener(OnButtonClick);
        }
          
        private void OnDisable()
        {
            _selectButon.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            onButtonClick?.Invoke(levelIndex);
        }

        public void SetStar()
        {
            starImage.enabled = true;
        }
    }
}
