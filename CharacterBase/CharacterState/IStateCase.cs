using System;
using CJH_Frame.AnimationBinder;
using CJH_Frame.StateMachines;
using StateCase;

public interface ICharacterState : IIdleState, ICastAnimationState, IRecoveryState, IDamagedState, IMoveState
{
    ICharacterStateMachineBinder CharacterStateMachineBinder { get; set; }

    public void AddLinks()
    {
        AddLink(IdleState, DamagedState);
        AddLink(IdleState, MoveState);
        AddLink(IdleState, CastAnimationState);

        AddLink(MoveState, IdleState);
        AddLink(MoveState, MoveState);
        AddLink(MoveState, CastAnimationState);
        AddLink(MoveState, DamagedState);

        AddLink(DamagedState, IdleState);

        AddLink(CastAnimationState, RecoveryState);

        AddLink(RecoveryState, IdleState);

        void AddLink(IStateCase src, IStateCase dev)
        {
            src.State.AddLink(new EventLink(new ActionWrapper(dev.SwitchState), dev.State));
        }
    }

    public void SetEvents()
    {
        IdleState.ActionOnStart.AddAction(CharacterStateMachineBinder.IdleAnimation);

        CastAnimationState.ActionOnStart.AddAction(() =>
            CharacterStateMachineBinder.AttackAnimation((RecoveryState as IStateCase).SetState));
        RecoveryState.ActionOnStart.AddAction(() =>
            CharacterStateMachineBinder.AttackAnimation((IdleState as IStateCase).SetState));

        DamagedState.ActionOnStart.AddAction(() =>
            CharacterStateMachineBinder.DamagedAnimation((IdleState as IStateCase).SetState));

        MoveState.ActionOnStart.AddAction(() => CharacterStateMachineBinder.MoveAnimation());
    }
}


namespace StateCase
{
    public interface IStateCase
    {
        public State State { get; }

        /// <summary>
        ///     listening to switch state to this
        /// </summary>
        public ActionPointer SwitchState { get; set; }


        /// <summary>
        ///     listening to switch state to this
        /// </summary>
        public ActionPointer ActionOnStart { get; set; }


        /// <summary>
        ///     listening to switch state to this
        /// </summary>
        public ActionPointer ActionOnExit { get; set; }

        public Action CheckAction { get; set; }

        public virtual void SetState()
        {
            SwitchState.Invoke();
            CheckAction.Invoke();
        }
    }

    #region IdleState

    public interface IIdleState
    {
        IdleState IdleState { get; set; }
    }

    public class IdleState : IStateCase
    {
        public IdleState(Action checkAction)
        {
            State = new State("IdleState", ActionOnStart.Invoke, ActionOnExit.Invoke);
            CheckAction = checkAction;
        }

        public State State { get; set; }
        public ActionPointer SwitchState { get; set; } = new();
        public ActionPointer ActionOnStart { get; set; } = new();
        public ActionPointer ActionOnExit { get; set; } = new();
        public Action CheckAction { get; set; }
    }

    #endregion


    #region CastAnimationState

    public interface ICastAnimationState
    {
        CastAnimationState CastAnimationState { get; set; }
    }

    public class CastAnimationState : IStateCase
    {
        private Action castskill;

        public CastAnimationState(Action checkAction)
        {
            State = new State("CastAnimationState", ActionOnStart.Invoke, ActionOnExit.Invoke);
            CheckAction = checkAction;
        }

        public State State { get; set; }
        public ActionPointer SwitchState { get; set; } = new();
        public ActionPointer ActionOnStart { get; set; } = new();
        public ActionPointer ActionOnExit { get; set; } = new();
        public Action CheckAction { get; set; }


        public void StartCast(Action _castskill)
        {
            castskill = _castskill;
        }
    }

    #endregion

    #region RecoveryState

    public interface IRecoveryState
    {
        RecoveryState RecoveryState { get; set; }
    }

    public class RecoveryState : IStateCase
    {
        public RecoveryState(Action checkAction)
        {
            State = new State("RecoveryState", ActionOnStart.Invoke, ActionOnExit.Invoke);
            CheckAction = checkAction;
        }

        public State State { get; set; }
        public ActionPointer SwitchState { get; set; } = new();
        public ActionPointer ActionOnStart { get; set; } = new();
        public ActionPointer ActionOnExit { get; set; } = new();
        public Action CheckAction { get; set; }
    }

    #endregion

    #region DamagedState

    public interface IDamagedState
    {
        DamagedState DamagedState { get; set; }
    }

    public class DamagedState : IStateCase
    {
        public DamagedState(Action checkAction)
        {
            State = new State("DamagedState", ActionOnStart.Invoke, ActionOnExit.Invoke);
            CheckAction = checkAction;
        }

        public State State { get; set; }
        public ActionPointer SwitchState { get; set; } = new();
        public ActionPointer ActionOnStart { get; set; } = new();
        public ActionPointer ActionOnExit { get; set; } = new();
        public Action CheckAction { get; set; }
    }

    #endregion

    #region MoveState

    public interface IMoveState
    {
        MoveState MoveState { get; set; }
    }

    public class MoveState : IStateCase
    {
        public MoveState(Action checkAction)
        {
            State = new State("MoveState", ActionOnStart.Invoke, ActionOnExit.Invoke);
            CheckAction = checkAction;
        }

        public State State { get; set; }
        public ActionPointer SwitchState { get; set; } = new();
        public ActionPointer ActionOnStart { get; set; } = new();
        public ActionPointer ActionOnExit { get; set; } = new();
        public Action CheckAction { get; set; }
    }

    #endregion

    #region DeadState

    public interface IDeadState
    {
        DeadState DeadState { get; set; }
    }

    public class DeadState : IStateCase
    {
        public DeadState(Action checkAction)
        {
            CheckAction = checkAction;
            //SwitchState.AddAction(ActionOnStart.Invoke);
            // State = new State("DeadState", ActionOnStart.Invoke, ActionOnExit.Invoke);
        }

        public State State { get; set; }
        public ActionPointer SwitchState { get; set; } = new();
        public ActionPointer ActionOnStart { get; set; } = new();
        public ActionPointer ActionOnExit { get; set; } = new();
        public Action CheckAction { get; set; }
    }

    #endregion
}