using DEPT.Unity;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class GroundedState : State<ILocomotionContext>
{
    private readonly StateMachine<ILocomotionContext> _subStateMachine;

    public GroundedState(StateMachine<ILocomotionContext> stateMachine) : base(stateMachine)
    {
        _subStateMachine = new StateMachine<ILocomotionContext>(StateMachine.Context);

        if (StateMachine.Context.Input.DirectionXZ.magnitude > 0f)
        {
            _subStateMachine.ChangeState<MovingState>();
        }
        else
        {
            _subStateMachine.ChangeState<IdleState>();
        }
    }

    public override void Update()
    {
        _subStateMachine.Update();

        if(!StateMachine.Context.IsGrounded)
        {
            StateMachine.ChangeState<FallingState>();
        }

        if (StateMachine.Context.Input.Jump)
        {
            StateMachine.ChangeState<JumpingState>();
        }
    }

    public override void Dispose()
    {
        _subStateMachine.Dispose();

        base.Dispose();
    }
}