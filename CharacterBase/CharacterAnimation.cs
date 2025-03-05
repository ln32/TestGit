using CJH_Frame.StateBinder.StateCase;

namespace CJH_Frame.AnimationBinder
{
    // 상태머신쪽에서 하위 인터페이스 객체를 대상으로 애니메이션 호출 할 예정.
    public interface ICharacterStateMachineBinder : IIdle, IAttack, IMove, IDamaged, IDead
    {
    }

    public interface IMonsterStateMachineBinder : IIdle, IAttack, IMove, IGroggy, IDead
    {
    }

    public interface IPetStateMachineBinder : IIdle, IAttack, IMove, IReact
    {
    }
}


namespace CJH_Frame.StateBinder.StateCase
{
    public interface IIdle
    {
        /// <summary>
        ///     대기 애니메이션
        ///     루프
        ///     상태머신쪽에서 애니메이션을 끊을것으로 예상 됨.
        /// </summary>
        public void IdleAnimation();
    }

    public interface IMove
    {
        /// <summary>
        ///     이동 애니메이션
        ///     루프
        ///     상태머신쪽에서 애니메이션을 끊을것으로 예상 됨.
        /// </summary>
        public void MoveAnimation();
    }

    public interface IAttack
    {
        /// <summary>
        ///     공격 애니메이션
        ///     1회성, 콜백 존재 콜백 2개로 해야할 지 고민중.
        /// </summary>
        /// <param name="callback"> 애니메이션 종료 시 호출 </param>
        public void AttackAnimation(Action callback);
    }

    public interface IDamaged
    {
        /// <summary>
        ///     피격 애니메이션
        ///     1회성, 콜백
        /// </summary>
        /// <param name="callback"> 애니메이션 종료 시 호출 </param>
        public void DamagedAnimation(Action callback);
    }

    public interface IDead
    {
        /// <summary>
        ///     사망 애니메이션
        ///     1회성, 콜백
        /// </summary>
        /// <param name="callback"> 애니메이션 종료 시 호출 </param>
        public void DeadAnimation(Action callback);
    }

    public interface IGroggy
    {
        /// <summary>
        ///     그로기 애니메이션
        ///     루프
        ///     상태머신쪽에서 애니메이션을 끊을것으로 예상 됨.
        /// </summary>
        /// <param name="callback"> 애니메이션 종료 시 호출 </param>
        public void GroggyAnimation(Action callback);
    }

    public interface IReact
    {
        /// <summary>
        ///     상호작용, 기획서 아직 안정해짐, 펫 전용
        ///     감히 짧게 예측해보자면 레벨업 세레모니 같은 교감 관련 애니메이션 예상
        /// </summary>
        /// <param name="callback"> 애니메이션 종료 시 호출 </param>
        public void ReactAnimation(Action callback);
    }
}
