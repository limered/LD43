using System.Collections;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Combat
{
    public class CollisionDamageRecieverComponent : GameComponent
    {
        public ReactiveCommand RecievedDamage = new ReactiveCommand();

        public float ShakeStrength = .1f;
        public float ShakeDecay = 0.005f;

        private float _shakeDecay;
        private float _shakeIntensity;

        private Vector3 _originPosition;
        private Quaternion _originRotation;
        private Transform _transform;

        private void OnEnable()
        {
            _transform = GetComponentInChildren<Animator>().transform;
        }

        private IEnumerator ShakeIt()
        {
            while (_shakeIntensity > 0f)
            {
                print("f");
                _transform.localPosition = _originPosition + Random.insideUnitSphere * _shakeIntensity;
                _transform.localRotation = new Quaternion(
                    _originRotation.x + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f,
                    _originRotation.y + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f,
                    _originRotation.z + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f,
                    _originRotation.w + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f);
                _shakeIntensity -= ShakeDecay;
                yield return null;
            }

            ShakingStopped();

            yield return null;
        }

        private void ShakingStopped()
        {
            _transform.localPosition = _originPosition;
            _transform.localRotation = _originRotation;
        }

        public void Shake()
        {
            _originPosition = _transform.localPosition;
            _originRotation = _transform.localRotation;

            _shakeIntensity = ShakeStrength;
            StartCoroutine("ShakeIt");
        }
    }
}
