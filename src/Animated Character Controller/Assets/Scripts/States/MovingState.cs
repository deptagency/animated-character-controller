namespace DEPT.Unity
{
    public class MovingState : State<ILocomotionContext>
    {
        public MovingState()
        {

        }

        public override void Update()
        {
            StateMachine.Context.ApplyInputTranslationXZ(5f);
            StateMachine.Context.ApplyInputRotationY(10f);
        }
    }
}
