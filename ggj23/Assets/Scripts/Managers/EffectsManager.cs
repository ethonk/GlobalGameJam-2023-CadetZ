using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EffectsManager : MonoBehaviour
    {
        // effects manager instance
        private static EffectsManager _instance;
        public static EffectsManager Instance { get { return _instance; }}

        [Header("Shake Settings")]
        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeMultiplier = 0.7f;

        [Header("References")]
        [SerializeField] private Transform objectToShake;
        
        //
        private float _currShakeDuration;
        private bool _shaking;
        private Vector3 _originalPos;

        private void Start()
        {
            _originalPos = objectToShake.localPosition;
        }

        /// <summary>
        /// Ensures that the Effects Manager only ever exists once.
        /// </summary>
        private void Awake()
        {
            // set the instance
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;
        }

        private void Update()
        {
            if (!_shaking) return;
            
            // shake
            objectToShake.localPosition = _originalPos + Random.insideUnitSphere * shakeMultiplier;
            // increment timer
            _currShakeDuration += Time.deltaTime;
            if (_currShakeDuration > shakeDuration)
            {
                _currShakeDuration = 0f;
                _shaking = false;
            }
        }

        public void Shake()
        {
            _shaking = true;
        }
    }
}
