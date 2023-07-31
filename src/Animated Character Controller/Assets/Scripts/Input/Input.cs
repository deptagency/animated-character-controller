using UnityEngine;
using UnityEngine.InputSystem;

namespace DEPT.Unity
{


    public class Input : MonoBehaviour
    {
        public const float RUN_SPEED = 1f;
        public const float WALK_SPEED = 0.5f;

        public Transform Camera;
        private SceneBehavior _sceneBehavior;
        private Vector3 _inputDirection = Vector3.zero;

        public bool Run = false;
        public bool Jump = false;
        public Vector3 MovementXZ = Vector3.zero;
        public float SpeedXZ = 1f;
        public Quaternion RotationY = Quaternion.identity;

        public void Awake()
        {
            _sceneBehavior = GetComponent<SceneBehavior>();
        }

        public void Update()
        {
            MovementXZ = (Quaternion.AngleAxis(Camera.rotation.eulerAngles.y, -_sceneBehavior.GravityDirection) * _inputDirection).normalized;

            if (MovementXZ != Vector3.zero)
            {
                RotationY = Quaternion.LookRotation(MovementXZ, -_sceneBehavior.GravityDirection);
            }
            else
            {
                RotationY = Quaternion.identity;
            }

            SpeedXZ = Mathf.Clamp01(_inputDirection.magnitude) * (Run ? RUN_SPEED : WALK_SPEED);
        }

        public void OnHorizontalMovement(InputValue value)
        {
            var input = value.Get<Vector2>();

            _inputDirection = new Vector3(input.x, 0f, input.y);
        }

        public void OnJump(InputValue value)
        {
            Jump = value.isPressed;
        }
    }
}
