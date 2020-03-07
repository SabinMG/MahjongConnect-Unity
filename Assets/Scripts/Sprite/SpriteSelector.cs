using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    public class SpriteSelector : MonoBehaviour
    {
        public Color selectedColor;
        public Color unselectedColor;
        public GameManager gameManager;
        public LayerMask spriteViewMask;
        private Camera _mainCamera;

        void Start()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (!gameManager.GameRunning) return;

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, spriteViewMask))
                {
                    SpriteView spriteView = hit.collider.gameObject.GetComponent<SpriteView>();
                    if(spriteView != null) gameManager.SelectObject(spriteView.SpriteObj, selectedColor, unselectedColor);
                }
            }
        }
    }
}
