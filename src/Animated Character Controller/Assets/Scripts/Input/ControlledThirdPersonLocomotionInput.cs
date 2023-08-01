using UnityEngine;
using UnityEngine.InputSystem;

namespace DEPT.Unity
{
    public class ControlledThirdPersonLocomotionInput : MonoBehaviour, ILocomotionInput
    {
        // Editor Settings
        public Transform Camera;
        [Range(0f, 0.5f)] public float JumpButtonGracePeriod = 0.2f;
        [Range(0f, 1f)] public float WalkSpeed = 0.5f;
        [Range(0f, 1f)] public float RunSpeed = 1f;

        // Private variables
        private Vector3 _inputDirection = Vector3.zero;
        private float? _mostRecentJumpPress;
        private bool _run = false;

        // Public variables
        public Vector3 DirectionXZ { get; private set; } = Vector2.zero;
        public float SpeedXZ { get; private set; } = 0f;
        public Quaternion RotationY { get; private set; } = Quaternion.identity;
        public bool Jump => _mostRecentJumpPress + JumpButtonGracePeriod >= Time.time;

        public void Update()
        {
            DirectionXZ = (Quaternion.AngleAxis(Camera.rotation.eulerAngles.y, Vector3.up) * _inputDirection).normalized;

            RotationY = DirectionXZ != Vector3.zero ? Quaternion.LookRotation(DirectionXZ, Vector3.up) : Quaternion.identity;

            SpeedXZ = Mathf.Clamp01(_inputDirection.magnitude) * (_run ? RunSpeed : WalkSpeed);
        }

        public void OnMovement(InputValue value)
        {
            var input = value.Get<Vector2>();

            _inputDirection = new Vector3(input.x, 0f, input.y);
        }

        public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                _mostRecentJumpPress = Time.time;
            }
        }

        public void OnRun(InputValue value)
        {
            _run = value.isPressed;
        }
    }
}
