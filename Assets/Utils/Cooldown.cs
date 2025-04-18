using UnityEngine;

namespace Utils
{
    public class Cooldown
    {
        private float duration;
        private float endTime;

        public Cooldown(float duration)
        {
            this.duration = duration;
            endTime = Time.time;
        }

        public bool IsExpired => Time.time >= endTime;

        public bool ResetIfExpired()
        {
            if (IsExpired)
            {
                endTime = Time.time + duration;
                return true;
            }
            return false;
        }

        public void SetExpired() => endTime = Time.time;

        public void Reset() => endTime = Time.time + duration;

        /// <summary>
        /// Sets the duration, does not reset the cooldown
        /// </summary>
        public void SetDuration(float duration) => this.duration = duration;

        public float GetRemainingTime() => Mathf.Max(endTime - Time.time, 0);
    }
}
