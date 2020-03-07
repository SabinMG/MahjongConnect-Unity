using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceOrigin.ObjectPool;

namespace SpaceOrigin.MahjongConnect
{
    public class SpriteDestroyEffect : MonoBehaviour
    {

        private ParticleSystem _particelSystem;

        public void Awake()
        {
            _particelSystem = GetComponent<ParticleSystem>();
        }

        public void PlayEffect()
        {
            _particelSystem.Play();
        }

        public void DestroyAfterSomeTime(float time)
        {
            Invoke("DestroyEffect", time);
        }

        private void DestroyEffect()
        {
            PoolManager.Instance.PutObjectBacktoPool(this.gameObject);
        }
    }
}
