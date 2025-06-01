using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using Utils;

namespace DebugUI
{
    public class DebugInfo : MonoBehaviour
    {
        private TextMeshProUGUI _informationGui;

        // Constants may be serialized for flexibility
        private readonly Dictionary<string, string> _extraInfo = new();
        private bool _extraInfoChanged;

        private float _currentAvg;
        private float _currentP1;
        private float _currentP01;
        private const float FPSUpdateInterval = 2f; // in seconds
        private Cooldown _fpsUpdateCooldown;

        private const float BufferDuration = 10f; // in seconds

        private readonly LinkedList<(float, float)> _frameTimeStamps = new();

        private void Awake()
        {
            _informationGui = GetComponent<TextMeshProUGUI>();
            _fpsUpdateCooldown = new Cooldown(FPSUpdateInterval);
            _fpsUpdateCooldown.Reset(); // make sure there will be data for the first update

            // Simple config check
            Debug.Assert(BufferDuration > 0f);
        }

        private void Update()
        {
            var time = Time.unscaledTime;
            _frameTimeStamps.AddFirst((Time.unscaledDeltaTime, time));
            var endTimeStamp = time - BufferDuration;
            while (_frameTimeStamps.Last.Value.Item2 < endTimeStamp)
                _frameTimeStamps.RemoveLast();

            var isFpsUpdated = _fpsUpdateCooldown.ResetIfExpired();
            if (_extraInfoChanged || isFpsUpdated)
                UpdateInfo(isFpsUpdated);
        }

        private void UpdateInfo(bool isFpsUpdated)
        {
            if (isFpsUpdated)
            {
                var framesCount = _frameTimeStamps.Count;
                _currentAvg =
                    framesCount
                    / (_frameTimeStamps.First.Value.Item2 - _frameTimeStamps.Last.Value.Item2);

                // Measuring 1% and 0.1% worst frames:
                // 1% is avg of 1% except worst
                // 0.1% is avg of second and third worst if atleast 3 frames, else avg of first and second
                // fun fact - this 0.1% is not 0.1% but no one cares

                var worstFramesCountP1 = framesCount / 100;
                if (worstFramesCountP1 > 1)
                {
                    var worstFrames = _frameTimeStamps
                        .OrderByDescending(x => x.Item1)
                        .Take(worstFramesCountP1);

                    _currentP1 = 1f / worstFrames.Skip(1).Average(x => x.Item1);

                    var currentP01Delta =
                        worstFramesCountP1 >= 3
                            ? worstFrames.Skip(1).Take(2).Average(x => x.Item1)
                            : worstFrames.Average(x => x.Item1);
                    _currentP01 = 1f / currentP01Delta;
                }
                else
                    _currentP1 = _currentP01 = _frameTimeStamps.Min(x => x.Item1);
            }

            var text = new StringBuilder(27); // optimal size for no extra info

            text.AppendLine($"FPS: {_currentAvg:F0}   1% {_currentP1:F0}   0.1% {_currentP01:F0}");

            foreach (var (key, value) in _extraInfo)
                text.AppendLine($"{key}: {value}");

            _informationGui.text = text.ToString();
            _extraInfoChanged = false;
        }

        public void SetExtraInfo(string key, string value)
        {
            _extraInfo[key] = value;
            _extraInfoChanged = true;
        }
    }
}
