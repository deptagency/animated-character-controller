using UnityEngine;

namespace DEPT.Unity
{
    public class LandingState : State<ILocomotionContext>
    {
        public LandingState(StateMachine<ILocomotionContext> stateMachine) : base(stateMachine)
        {
            StateMachine.Context.Animator.CrossFadeInFixedTime(Animator.StringToHash("Landing"));
        }

        public override void Update()
        {
            if (StateMachine.ElapsedTime > 0.5f - StateMachine.Context.Input.SpeedXZ)
            {
                StateMachine.ChangeState<GroundedState>();
            }
        }
    }
}