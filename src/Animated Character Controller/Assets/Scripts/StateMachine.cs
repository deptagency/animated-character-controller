using System;
using UnityEngine;

namespace DEPT.Unity
{
    public class StateMachine<TContext> : IDisposable
    {
        private State<TContext> _currentState;

        public TContext Context { get; private set; }
        public float ElapsedTime { get; protected set; }

        public StateMachine(TContext context)
        {
            Context = context;
        }

        public void ChangeState<T>() where T : State<TContext>
        {
            if(_currentState?.GetType() != typeof(T))
            {
                _currentState?.Dispose();

                ElapsedTime = 0f;

                _currentState = (T)Activator.CreateInstance(typeof(T), this);
            }
        }

        public virtual void Update()
        {
            ElapsedTime += Time.deltaTime;
            _currentState?.Update();
        }

        public void Dispose()
        {
            _currentState?.Dispose();
        }
    }
}
