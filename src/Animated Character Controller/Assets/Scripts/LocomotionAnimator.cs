using UnityEngine;

namespace DEPT.Unity
{
    [RequireComponent(typeof(Animator))]
    public class LocomotionAnimator : MonoBehaviour
    {
        private Animator _animator;

        private int _speedXZHash;
        private int _speedYHash;
        private Vector3 _cumulativeDeltaPosition = Vector3.zero;

        public void Awake()
        {
            _animator = GetComponent<Animator>();

            _speedXZHash = Animator.StringToHash("SpeedXZ");
            _speedYHash = Animator.StringToHash("SpeedY");
        }

        public void OnAnimatorMove()
        {
            _cumulativeDeltaPosition += _animator.deltaPosition;
        }

        public Vector3 ProcessRootMotionTranslation()
        {
            var rootMotion = _cumulativeDeltaPosition;

            _cumulativeDeltaPosition = Vector3.zero;

            return rootMotion;
        }

        public void SetSpeedXZ(float speedXZ, float deltaTime)
        {
            _animator.SetFloat(_speedXZHash, Mathf.Clamp(speedXZ, 0f, 1f), 0.1f, deltaTime);
        }

        public void SetSpeedY(float speedY, float deltaTime)
        {
            _animator.SetFloat(_speedYHash, Mathf.Clamp(speedY, 0f, 1f), 0.1f, deltaTime);
        }

        public void CrossFadeInFixedTime(int stateHashName, float fixedTransitionDuration = 0.1f)
        {
            _animator.CrossFadeInFixedTime(stateHashName, fixedTransitionDuration);
        }
    }
}