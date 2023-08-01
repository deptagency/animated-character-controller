using UnityEngine;

namespace DEPT.Unity
{
    public class LocomotionController : MonoBehaviour, ILocomotionContext
    {
        private CharacterController _characterController;
        private StateMachine<ILocomotionContext> _stateMachine;
        private Vector3 _cumulativeTranslation = Vector3.zero;

        public CollisionFlags Flags { get; private set; } = CollisionFlags.None;
        public ILocomotionInput Input { get; private set; }
        public Animator Animator { get; private set; }

        public void Start()
        {
            Input = GetComponent<ILocomotionInput>();
            Animator = GetComponentInChildren<Animator>();

            _characterController = GetComponent<CharacterController>();
            _stateMachine = new StateMachine<ILocomotionContext>(this);
            _stateMachine.ChangeState<MovingState>();
        }

        public void Update()
        {
            _stateMachine.Update(Time.deltaTime);

            Flags = _characterController.Move(_cumulativeTranslation);

            _cumulativeTranslation = Vector3.zero;
        }

        public void ApplyInputRotationY(float speedMultiplier = 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Input.RotationY, speedMultiplier * Time.deltaTime);
        }

        public void ApplyInputTranslationXZ(float speedMultiplier = 1f)
        {
            _cumulativeTranslation += Input.SpeedXZ * speedMultiplier * Time.deltaTime * Input.DirectionXZ;
        }
    }
}
