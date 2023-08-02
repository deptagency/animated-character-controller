using UnityEngine;

namespace DEPT.Unity
{
    public interface ILocomotionContext
    {
        public ILocomotionInput Input { get; }
        public LocomotionAnimator Animator { get; }

        public bool IsGrounded { get; }
        public Vector3 CumulativeVelocity { get; }

        public void ApplyInputTranslationXZ(float speedMultiplier = 1f);
        public void ApplyRootMotionTranslation();
        public void ApplyInputRotationY();
        public void ApplyJump();
    }
}