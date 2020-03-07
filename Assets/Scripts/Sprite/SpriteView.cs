using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceOrigin.ObjectPool;

namespace SpaceOrigin.MahjongConnect
{
    // spriteView renders the spriteobject
    public class SpriteView : MonoBehaviour
    {
        public SpriteRenderer spriterRender;
        private SpriteObject _spriteObject;
        private bool _showHint;
        private Vector3 _spriteInitRotation;
        private float _shakeSpeed = 20.0f;
        private float _shakeAmplitude = 6.0f;

        public SpriteObject SpriteObj { get => _spriteObject; set => _spriteObject = value; }

        void Start()
        {
            _spriteInitRotation = transform.eulerAngles;
        }

        void Update()
        {
            if (_showHint)
            {
                float zRot = Mathf.Sin(Time.time* _shakeSpeed) * _shakeAmplitude;
                Vector3 newRotation = _spriteInitRotation;
                newRotation.z += zRot;
                transform.eulerAngles = newRotation;
            }
        }

        public void LinkView(SpriteObject spriteObject)
        {
            _spriteObject = spriteObject;
            transform.position = _spriteObject.ParentCell.CellCenterPosition;
            _spriteInitRotation = transform.eulerAngles;
            _spriteObject.onSelectedObject += OnSelectView;
            _spriteObject.onUnselectObject += OnUnSelectView;
            _spriteObject.onDestroyObject += OnDestroyView;
            _spriteObject.onEnableHint += OnEnableHint;
            _spriteObject.onDesableHint += OnDesableHint;
        }

        public void OnSelectView(Color selectedColor)
        {
            spriterRender.color = selectedColor;
        }

        public void OnUnSelectView(Color unselectedColor)
        {
            spriterRender.color = unselectedColor;
        }

        public void OnEnableHint()
        {
            _showHint = true;
        }

        public void OnDesableHint()
        {
            transform.eulerAngles = _spriteInitRotation;
        }

        public void OnDestroyView(Color unselectedColor)
        {
            spriterRender.color = unselectedColor;
            _spriteObject.onSelectedObject -= OnSelectView;
            _spriteObject.onUnselectObject -= OnUnSelectView;
            _spriteObject.onDestroyObject -= OnDestroyView;
            _spriteObject.onEnableHint -= OnEnableHint;
            _spriteObject.onDesableHint -= OnDesableHint;
            _spriteObject = null;

            GameObject destroyEffect = PoolManager.Instance.GetObjectFromPool("SpriteDestroyEffect");
            SpriteDestroyEffect effectObj = destroyEffect.GetComponent<SpriteDestroyEffect>();
            destroyEffect.transform.position = transform.position;
            destroyEffect.SetActive(true);
            effectObj.PlayEffect();
            effectObj.DestroyAfterSomeTime(2.0f);
            gameObject.SetActive(false);
        }
    }
}
