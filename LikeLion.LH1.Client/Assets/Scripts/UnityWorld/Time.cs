using LikeLion.LH1.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeLion.LH1.Client.UnityWorld
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
