using System;

namespace DEPT.Unity
{
    public abstract class State<TContext> : IDisposable
    {
        public StateMachine<TContext> StateMachine { get; set; }

        public virtual void Update()
        {

        }

        public virtual void Dispose()
        {
            
        }
    }
}
