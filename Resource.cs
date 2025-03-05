using System.Numerics;

namespace CJH_Frame
{
    internal class Resource
    {
    }
}

// 5. 효과 & 사운드
public interface IEffectSystem
{
    void ApplyEffect(string effectID, Vector3 target);
    void RemoveEffect(string effectID);
}

public interface ISoundPlayer
{
    void PlaySFX(string soundKey);
    void StopSFX(string soundKey);
}

