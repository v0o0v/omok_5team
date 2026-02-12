using Omok;
using System;

namespace Omok
{
    public class Time : ITime
    {
        public long GetCurrentTimeMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public long GetCurrentTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public float GetDeltaTime()
        {
            return UnityEngine.Time.deltaTime;
        }

        public long GetToday()
        {
            DateTime todayUtc = DateTime.UtcNow.Date;
            return new DateTimeOffset(todayUtc).ToUnixTimeSeconds();
        }
    }
}
