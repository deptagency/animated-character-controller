using UnityEngine;

namespace DEPT.Unity
{
    public class JumpingState : State<ILocomotionContext>
    {
        public JumpingState(StateMachine<ILocomotionContext> stateMachine) : base(stateMachine)
        {
            StateMachine.Context.Animator.CrossFadeInFixedTime(Animator.StringToHash("Jumping"), 0.1f);
            
            StateMachine.Context.ApplyJump();
        }

        public override void Update()
        {
            StateMachine.Context.ApplyInputTranslationXZ(5f);

            if (StateMachine.Context.CumulativeVelocity.y < 0f)
            {
                StateMachine.ChangeState<FallingState>();
            }
        }
    }
}