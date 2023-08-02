using UnityEngine;

namespace DEPT.Unity
{
    public class MovingState : State<ILocomotionContext>
    {
        public MovingState(StateMachine<ILocomotionContext> stateMachine) : base(stateMachine)
        {
            StateMachine.Context.Animator.CrossFadeInFixedTime(Animator.StringToHash("Moving"), 0.1f);
        }

        public override void Update()
        {
            StateMachine.Context.ApplyRootMotionTranslation();
            StateMachine.Context.ApplyInputRotationY();

            if (StateMachine.Context.Input.DirectionXZ.magnitude == 0)
            {
                StateMachine.ChangeState<IdleState>();
            }
        }
    }

}