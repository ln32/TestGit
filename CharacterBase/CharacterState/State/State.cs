using System;
using System.Collections;

namespace CJH_Frame.StateMachines
{
    /// <summary>
    ///     A generic empty state. Pass onExecute action into Constructor to run once when entering the state
    ///     (or null to do nothing)
    /// </summary>
    public class State : AbstractState
    {
        private readonly Action m_OnExecute;
        private readonly Action m_OnStart;


        /// <param name="onExecute">An event that is invoked when the state is executed</param>
        // Constructor takes delegate to execute and optional name (for debugging)
        public State(string stateName, Action onStart, Action onExecute)
        {
            m_OnExecute = onExecute;
            m_OnStart = onStart;
            Name = stateName;
        }


        public override void Enter()
        {
            m_OnStart?.Invoke();
            EnableLinks();
        }

        public override IEnumerator Execute()
        {
            yield return null;

            // Invokes the m_OnExecute Action if it exists
            m_OnExecute?.Invoke();
        }
    }
}