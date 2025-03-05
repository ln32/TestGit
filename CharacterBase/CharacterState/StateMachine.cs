namespace CJH_Frame.StateMachines
{
    /// <summary>
    ///     A Generic state machine, adapted from the Runner template
    ///     https://unity.com/features/build-a-runner-game
    /// </summary>
    public class StateMachine
    {
        protected bool m_PlayLock;

        // The current state the statemachine is in
        public IState? CurrentState { get; protected set; }

        public bool IsRunning { get; protected set; }

        /// <summary>
        ///     Finalizes the previous state and then runs the new state
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void SetCurrentState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (CurrentState != null && IsRunning)
                //interrupt currently executing state
                Skip();

            CurrentState = state;
            m_PlayLock = true;

            IsRunning = (state == null);
        }

        /// <summary>
        ///     Interrupts the execution of the current state and finalizes it.
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected void Skip()
        {
            if (CurrentState == null)
                throw new Exception($"{nameof(CurrentState)} is null!");

            if (IsRunning)
            {
                //finalize current state
                CurrentState.Exit();
                IsRunning = false;
                m_PlayLock = false;
            }
        }

        public virtual void Run(IState state)
        {
            SetCurrentState(state);
            Run();
        }

        public virtual void Run()
        {
            if (IsRunning)//already running
                return;

            IsRunning = true; 
        }

        /// <summary>
        ///     Turns off the main loop of the StateMachine
        /// </summary>
        public void Stop()
        {
            if (IsRunning == false)//already running
                return;

            if (CurrentState != null && IsRunning)
                Skip();

            CurrentState = null;
        }

        public void AssignState()
        {
            if (CurrentState != null && IsRunning == false) //current state is done playing
                if (CurrentState.ValidateLinks(out var nextState))
                {
                    if (m_PlayLock)
                    {
                        //finalize current state
                        CurrentState.Exit();
                        m_PlayLock = false;
                    }

                    CurrentState.DisableLinks();
                    SetCurrentState(nextState);
                    CurrentState.EnableLinks();
                }
        }
    }
}

public class ActionPointer
{
    private Action Action { get; set; }

    public ActionPointer() { }

    public ActionPointer(ref Action _Action)
    {
        _Action += () => { Action?.Invoke(); };
    }


    public void AddAction(Action target)
    {
        Action += target;
    }

    public void RemoveAction(Action target)
    {
        Action -= target;
        Action -= () => { Action?.Invoke(); };
        Action = null;
    }

    public void Invoke()
    {
        Action?.Invoke();
    }
}