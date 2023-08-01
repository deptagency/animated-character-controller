namespace DEPT.Unity
{
    public class StateMachine<TContext>
    {
        private State<TContext> _currentState;

        public TContext Context { get; private set; }
        public float ElapsedTime { get; protected set; }

        public StateMachine(TContext context)
        {
            Context = context;
        }

        public void ChangeState<T>() where T : State<TContext>, new()
        {
            if(_currentState?.GetType() != typeof(T))
            {
                _currentState?.Dispose();

                ElapsedTime = 0f;
                _currentState = new T
                {
                    StateMachine = this
                };
            }
        }

        public virtual void Update(float deltaTime)
        {
            ElapsedTime += deltaTime;
            _currentState?.Update();
        }
    }
}
