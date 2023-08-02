using System;

namespace DEPT.Unity
{
    public abstract class State<TContext> : IDisposable
    {
        protected StateMachine<TContext> StateMachine { get; }

        public State(StateMachine<TContext> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Update()
        {

        }

        public virtual void Dispose()
        {
            
        }
    }
}
