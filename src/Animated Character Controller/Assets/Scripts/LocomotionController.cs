using System.Security.Cryptography;
using UnityEngine;

namespace DEPT.Unity
{
    public class LocomotionController : MonoBehaviour, ILocomotionContext
    {
        private CharacterController _characterController;
        private StateMachine<ILocomotionContext> _stateMachine;
        private Vector3 _cumulativeTranslation = Vector3.zero;
        private float _initialStepOffset;
        private Vector3 _gravity = Physics.gravity;

        [Range(1f, 20f)] public float RotationMultiplier = 10f;
        [Range(0.1f, 5f)] public float GravityMultiplier = 2f;
        [Range(0f, 100f)] public float TerminalVelocity = 50f;

        public bool IsGrounded => _characterController.isGrounded;
        public Vector3 CumulativeVelocity { get; private set; }
        public CollisionFlags Flags { get; private set; } = CollisionFlags.None;
        public ILocomotionInput Input { get; private set; }
        public LocomotionAnimator Animator { get; private set; }

        public void Awake()
        {
            Input = GetComponent<ILocomotionInput>();
            Animator = GetComponentInChildren<LocomotionAnimator>();
            _characterController = GetComponent<CharacterController>();
        }

        public void Start()
        {
            _stateMachine = new StateMachine<ILocomotionContext>(this);

            _stateMachine.ChangeState<GroundedState>();

            _initialStepOffset = _characterController.stepOffset;
        }

        public void Update()
        {
            Animator.SetSpeedXZ(Input.SpeedXZ, Time.deltaTime);
            Animator.SetSpeedY(CumulativeVelocity.magnitude / TerminalVelocity, Time.deltaTime);

            _stateMachine.Update();

            ApplyVelocity();

            Flags = _characterController.Move(_cumulativeTranslation);

            _cumulativeTranslation = Vector3.zero;
        }

        private void ApplyVelocity()
        {
            if (IsGrounded && CumulativeVelocity.y < 0f)
            {
                CumulativeVelocity = new Vector3(CumulativeVelocity.x, -_initialStepOffset, CumulativeVelocity.z);
            }
            else
            {
                CumulativeVelocity = Vector3.ClampMagnitude(CumulativeVelocity + (_gravity * GravityMultiplier * Time.deltaTime), TerminalVelocity);
            }

            _cumulativeTranslation += CumulativeVelocity * Time.deltaTime;
        }

        public void ApplyVelocity(Vector3 velocity)
        {
            CumulativeVelocity += velocity;
        }

        public void ApplyInputRotationY()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Input.RotationY, RotationMultiplier * Time.deltaTime);
        }

        public void ApplyInputTranslationXZ(float speedMultiplier = 1f)
        {
            _cumulativeTranslation += Input.SpeedXZ * speedMultiplier * Time.deltaTime * Input.DirectionXZ;
        }

        public void ApplyRootMotionTranslation()
        {
            _cumulativeTranslation += Animator.ProcessRootMotionTranslation();
        }
    }
}
