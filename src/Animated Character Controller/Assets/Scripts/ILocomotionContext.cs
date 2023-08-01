using UnityEngine;

namespace DEPT.Unity
{
    public interface ILocomotionContext
    {
        public ILocomotionInput Input { get; }
        public Animator Animator { get; }

        public void ApplyInputTranslationXZ(float speedMultiplier = 1f);
        public void ApplyInputRotationY(float speedMultiplier = 1f);
    }
}
