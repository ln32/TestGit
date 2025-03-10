using System;

namespace CJH_Frame.StateMachines
{
    // Encapsulate the delegate in a class so we can later unregister the Action

    // Note: you can alternatively forego the use of a wrapper if you guarantee that each static delegate
    // contains a default delegate
    public class ActionWrapper
    {
        public ActionWrapper(ref Action targetAction)
        {
            var actionPointer = new ActionPointer(ref targetAction);
            Subscribe += enableFlag => { actionPointer.AddAction(enableFlag); };
            Unsubscribe += enableFlag => { actionPointer.RemoveAction(enableFlag); };
        }

        public ActionWrapper(ActionPointer actionPointer)
        {
            Subscribe += enableFlag => { actionPointer.AddAction(enableFlag); };
            Unsubscribe += enableFlag => { actionPointer.RemoveAction(enableFlag); };
        }

        public Action<Action> Subscribe { get; set; }
        public Action<Action> Unsubscribe { get; set; }
    }


    /// <summary>
    ///     A link that listens for a specific event and becomes open for transition if the event is raised.
    ///     If the current state is linked to next step by this link type, the state machine waits for the
    ///     event to be triggered and then moves to the next step.
    /// </summary>
    public class EventLink : ILink
    {
        // Wraps the event message for easier unregistration
        private readonly ActionWrapper m_ActionWrapper;
        private readonly IState m_NextState;

        private bool m_EventRaised;

        public EventLink(ActionWrapper eventActionWrapper, IState nextState)
        {
            m_ActionWrapper = eventActionWrapper;
            m_NextState = nextState;
        }

        public bool Validate(out IState nextState)
        {
            nextState = m_EventRaised ? m_NextState : null;
            return m_EventRaised;
        }

        public void Enable()
        {
            m_ActionWrapper.Subscribe(OnEventRaised);
            m_EventRaised = false;
        }

        public void Disable()
        {
            m_ActionWrapper.Unsubscribe(OnEventRaised);
            m_EventRaised = false;
        }

        public void OnEventRaised()
        {
            m_EventRaised = true;
        }
    }
}