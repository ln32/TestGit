namespace UnityHFSM
{
    /// <summary>
    /// Default timer that calculates the elapsed time based on Time.time.
    /// </summary>
    public class Timer : ITimer
	{
		public float startTime;
		public float Elapsed => 0;//Time.time - startTime;

		public void Reset()
		{
			startTime = 0;// Time.time;
		}

		public static bool operator >(Timer timer, float duration)
			=> timer.Elapsed > duration;

		public static bool operator <(Timer timer, float duration)
			=> timer.Elapsed < duration;

		public static bool operator >=(Timer timer, float duration)
			=> timer.Elapsed >= duration;

		public static bool operator <=(Timer timer, float duration)
			=> timer.Elapsed <= duration;
	}
}
