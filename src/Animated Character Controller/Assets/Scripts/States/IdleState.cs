using UnityEngine;

namespace DEPT.Unity
{
    public class IdleState : State<ILocomotionContext>
    {
        public IdleState(StateMachine<ILocomotionContext> stateMachine) : base(stateMachine)
        {
            StateMachine.Context.Animator.CrossFadeInFixedTime(Animator.StringToHash("Idle"), 0.1f);
        }

        public override void Update()
        {
            if (StateMachine.Context.Input.DirectionXZ.magnitude > 0f)
            {
                StateMachine.ChangeState<MovingState>();
            }
        }
    }
}