using UnityEngine;

namespace Utils
{
    public class Cooldown
    {
        private float _duration;
        private float _endTime;

        public Cooldown(float duration)
        {
            _duration = duration;
            _endTime = Time.time;
        }

        public bool IsExpired => Time.time > _endTime;

        public bool ResetIfExpired()
        {
            if (IsExpired)
            {
                _endTime = Time.time + _duration;
                return true;
            }
            return false;
        }

        public void Reset() => _endTime = Time.time + _duration;

        /// <summary>
        /// Sets the duration, does not reset the cooldown
        /// </summary>
        public void SetDuration(float duration) => _duration = duration;

        public float GetRemainingTime() => _endTime - Time.time;
    }
}
