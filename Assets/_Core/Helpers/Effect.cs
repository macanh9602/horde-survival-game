using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTLTools.Effect
{
    public class Effect : MonoBehaviour
    {
        [SerializeField] MainParticleSystem mainParticleSystem;
        [SerializeField] bool isDestroyAfterStop = false;
        [SerializeField] bool isUsePool = false;

        [ShowInInspector]
        public bool IsPlaying
        {
            get
            {
                if (mainParticleSystem != null)
                    return mainParticleSystem.ThisParticleSystem.isPlaying;
                else
                    return false;
            }
        }
        public void OnParticleSystemStoppedListener()
        {
            //DPDebug.Log($"<color=green>Ping</color>");
            if (isDestroyAfterStop)
                Destroy(this.gameObject);
            if (isUsePool)
            {
                ObjectPool.Recycle(this);
            }
        }

        public void Init(Vector3 _pos, Transform _parent = null)
        {
            this.transform.position = _pos;
            this.transform.parent = _parent;
        }

        public void DOMoveY(float _value, float _duration)
        {
            this.transform.DOMoveY(_value, _duration);
        }

        public void Play()
        {
            if (IsPlaying)
                return;
            mainParticleSystem.ThisParticleSystem.Play();
        }

        public void Stop()
        {
            mainParticleSystem.ThisParticleSystem.Stop();
        }

        public void Pause()
        {
            mainParticleSystem.ThisParticleSystem.Pause();
        }

        public void SetRateOverTime(float _value)
        {
            var emission = mainParticleSystem.ThisParticleSystem.emission;
            emission.rateOverTime = _value;
        }

        public void ChangeColor(Color _color)
        {
            var main = mainParticleSystem.ThisParticleSystem.main;
            main.startColor = _color;
        }

        //===================================================
        #region [EDITOR]
        [Button]
        public void SetUp()
        {
            ParticleSystem ps = TransformExtensions.GetOrAddComponent<ParticleSystem>(this.gameObject);
            MainParticleSystem mainParticleSystem = TransformExtensions.GetOrAddComponent<MainParticleSystem>(ps.gameObject);
            mainParticleSystem.effect = this;
            this.mainParticleSystem = mainParticleSystem;
        }
        #endregion
        //===================================================
    }
}