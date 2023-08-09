using UnityEngine;

namespace DEPT.Unity
{
    public class LocomotionController : MonoBehaviour, ILocomotionContext
    {
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        public LayerMask LayerMask;
        private StateMachine<ILocomotionContext> _stateMachine;
        private Vector3 _cumulativeAcceleration = Vector3.zero;
        private Vector3 _cumulativeTranslation = Vector3.zero;
        private Vector3 _gravity => Physics.gravity * GravityMultiplier;

        public IGravityField _activeGravityField { get; private set; }

        [Range(1f, 20f)] public float RotationMultiplier = 10f;
        [Range(0.1f, 5f)] public float GravityMultiplier = 2f;
        [Range(0f, 100f)] public float TerminalVelocity = 50f;
        [Range(0f, 10f)] public float JumpHeight = 2f;

        public bool IsGrounded { get; private set; } = true;
        public Vector3 CumulativeVelocity { get; private set; }
        public CollisionFlags Flags { get; private set; } = CollisionFlags.None;
        public ILocomotionInput Input { get; private set; }
        public LocomotionAnimator Animator { get; private set; }

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(_rigidbody.position, _rigidbody.position + _cumulativeAcceleration.normalized * 10f);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gravity"))
            {
                var gravityField = other.GetComponent<IGravityField>();

                _activeGravityField = gravityField;
            }
        }

        public void Awake()
        {
            Input = GetComponent<ILocomotionInput>();
            Animator = GetComponentInChildren<LocomotionAnimator>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
        }

        public void Start()
        {
            _stateMachine = new StateMachine<ILocomotionContext>(this);

            _stateMachine.ChangeState<GroundedState>();
        }

        public void Update()
        {
            Animator.SetSpeedXZ(Input.SpeedXZ, Time.deltaTime);
            Animator.SetSpeedY(CumulativeVelocity.magnitude / TerminalVelocity, Time.deltaTime);

            _stateMachine.Update();

            if (_activeGravityField == null)
            {
                ApplyAcceleration(Physics.gravity);
            }
            else
            {
                ApplyAcceleration(_activeGravityField.CalculateAcceleration(_rigidbody));
            }
        }

        public void FixedUpdate()
        {
            var surfaceNormal = transform.up;

            var pos = _rigidbody.position + _cumulativeAcceleration.normalized * (_collider.radius + 0.001f);
            var hitColliders = Physics.OverlapSphere(pos, _collider.radius, LayerMask);

            IsGrounded = hitColliders.Length > 0;

            //var verticalRotation = Vector3.Lerp(transform.up, -_cumulativeAcceleration.normalized, _cumulativeAcceleration.magnitude);
            //var rotation = Quaternion.FromToRotation(transform.up, verticalRotation) * _rigidbody.rotation;

            var horizontalRotation = Quaternion.LookRotation(Input.DirectionXZ, -_cumulativeAcceleration.normalized);
            var rotation = Quaternion.Slerp(_rigidbody.rotation, horizontalRotation, 3f);

            //var rotationUp = Quaternion.FromToRotation(transform.up, -_cumulativeAcceleration);
            //var rotationRight = Quaternion.LookRotation(Input.DirectionXZ != Vector3.zero ? Input.DirectionXZ : transform.forward, transform.up);
            _rigidbody.MoveRotation(rotation);

            ProcessAcceleration();
            ProcessVelocity();
            Move(surfaceNormal);
        }

        private void Move(Vector3 surfaceNormal)
        {
            var translation = _cumulativeTranslation;
            //var translation = Vector3.ProjectOnPlane(_cumulativeTranslation, surfaceNormal);

            if (PhysicsExtensions.SphereCast(_collider, translation, out var hitInfo, translation.magnitude, LayerMask))
            {
                Debug.Log("Unable to move without collision");
                translation = Vector3.ProjectOnPlane(translation.normalized * translation.magnitude, hitInfo.normal);

                if (PhysicsExtensions.SphereCast(_collider, translation, out hitInfo, translation.magnitude, LayerMask))
                {
                    Debug.Log("Unable to adjust movement without secondary collision");
                    translation = translation.normalized * (hitInfo.distance - 0.0001f);
                }
            }

            _rigidbody.MovePosition(_rigidbody.position + translation);

            // Reset cumulative movement vectors
            _cumulativeTranslation = Vector3.zero;
            CumulativeVelocity = Vector3.zero;
            _cumulativeAcceleration = Vector3.zero;
        }

        private void ProcessAcceleration()
        {
            CumulativeVelocity += _cumulativeAcceleration;
        }

        public void ApplyAcceleration(Vector3 acceleration)
        {
            _cumulativeAcceleration += acceleration * Time.deltaTime;
        }

        private void ProcessVelocity()
        {
            _cumulativeTranslation += Vector3.ClampMagnitude(CumulativeVelocity, TerminalVelocity);
        }

        public void ApplyVelocity(Vector3 velocity)
        {
            CumulativeVelocity += velocity * Time.deltaTime;
        }

        public void ApplyJump()
        {
            var jumpVelocity = new Vector3(0f, Mathf.Sqrt(JumpHeight * -3f * _gravity.y), 0f);

            ApplyVelocity(jumpVelocity);
        }

        public void ApplyInputRotationY()
        {
            //_rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Input.RotationY, RotationMultiplier * Time.deltaTime));
        }

        public void ApplyInputTranslationXY(float speedMultiplier = 1f)
        {
            _cumulativeTranslation += Input.SpeedXZ * speedMultiplier * Time.deltaTime * new Vector3(Input.RawDirectionXY.x, 0, Input.RawDirectionXY.y);
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
