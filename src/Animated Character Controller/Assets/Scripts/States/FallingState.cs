using UnityEngine;

namespace DEPT.Unity
{
    public class FallingState : State<ILocomotionContext>
    {
        public FallingState(StateMachine<ILocomotionContext> stateMachine) : base(stateMachine)
        {
            StateMachine.Context.Animator.CrossFadeInFixedTime(Animator.StringToHash("Falling"), 0.1f);
        }

        public override void Update()
        {
            StateMachine.Context.ApplyInputTranslationXZ(5f);

            if (StateMachine.Context.IsGrounded)
            {
                StateMachine.ChangeState<LandingState>();
            }
        }
    }
}