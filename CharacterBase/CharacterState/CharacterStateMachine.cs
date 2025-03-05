using CJH_Frame.AnimationBinder;
using CJH_Frame.StateMachines;
using StateCase;

public interface IStateMachine
{
    CharacterStateMachine GetStateMachine { get; }
}

// TODO : 내 상태 interface 시키기
public class CharacterStateMachine : ICharacterState
{
    public ICharacterStateMachineBinder _AnimationProcessor;

    public StateMachine m_StateMachine = new();


    private void Start()
    {
        SetAvailable();
        Initialize();
    }

    public ICharacterStateMachineBinder CharacterStateMachineBinder { get; set; }

    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public RecoveryState RecoveryState { get; set; }
    public DamagedState DamagedState { get; set; }
    public CastAnimationState CastAnimationState { get; set; }

    private void SetAvailable()
    {
        Action IsAvailable = m_StateMachine.AssignState;

        IdleState = new IdleState(IsAvailable);
        MoveState = new MoveState(IsAvailable);
        RecoveryState = new RecoveryState(IsAvailable);
        DamagedState = new DamagedState(IsAvailable);
        CastAnimationState = new CastAnimationState(IsAvailable);
    }


    private void Initialize()
    {
        CharacterStateMachineBinder = _AnimationProcessor;

        // Set up the coroutines helper for non-MonoBehaviours
        //Coroutines.Initialize(this);

        (this as ICharacterState).AddLinks();
        (this as ICharacterState).SetEvents();

        m_StateMachine.Run(IdleState.State);
    }


    public void Event_CastAttack(Action castSkill)
    {
        CastAnimationState.StartCast(castSkill);
        (CastAnimationState as IStateCase).SetState();
    }
}