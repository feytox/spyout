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
        private TextMeshProUGUI informationGui;

        // Constants may be serialized for flexibility
        private Dictionary<string, string> extraInfo = new();
        private bool extraInfoChanged = false;

        private float currentAvg;
        private float currentP1;
        private float currentP01;
        private const float FPS_UPDATE_INTERVAL = 2f; // in seconds
        private Cooldown fpsUpdateCooldown;

        private const float BUFFER_DURATION = 10f; // in seconds

        private LinkedList<(float, float)> frameTimeStamps = new();

        void Awake()
        {
            informationGui = GetComponent<TextMeshProUGUI>();
            fpsUpdateCooldown = new Cooldown(FPS_UPDATE_INTERVAL);
            fpsUpdateCooldown.Reset(); // make sure there will be data for the first update

            // Simple config check
            Debug.Assert(BUFFER_DURATION > 0f);
        }

        void Update()
        {
            var time = Time.unscaledTime;
            frameTimeStamps.AddFirst((Time.unscaledDeltaTime, time));
            var endTimeStamp = time - BUFFER_DURATION;
            while (frameTimeStamps.Last.Value.Item2 < endTimeStamp)
                frameTimeStamps.RemoveLast();

            var isFpsUpdated = fpsUpdateCooldown.ResetIfExpired();
            if (extraInfoChanged || isFpsUpdated)
                UpdateInfo(isFpsUpdated);
        }

        private void UpdateInfo(bool isFpsUpdated)
        {
            if (isFpsUpdated)
            {
                var framesCount = frameTimeStamps.Count;
                currentAvg =
                    framesCount
                    / (frameTimeStamps.First.Value.Item2 - frameTimeStamps.Last.Value.Item2);

                // Measuring 1% and 0.1% worst frames:
                // 1% is avg of 1% except worst
                // 0.1% is avg of second and third worst if atleast 3 frames, else avg of first and second
                // fun fact - this 0.1% is not 0.1% but no one cares

                var worstFramesCountP1 = framesCount / 100;
                if (worstFramesCountP1 > 1)
                {
                    var worstFrames = frameTimeStamps
                        .OrderByDescending(x => x.Item1)
                        .Take(worstFramesCountP1);

                    currentP1 = 1f / worstFrames.Skip(1).Average(x => x.Item1);

                    var currentP01Delta =
                        worstFramesCountP1 >= 3
                            ? worstFrames.Skip(1).Take(2).Average(x => x.Item1)
                            : worstFrames.Average(x => x.Item1);
                    currentP01 = 1f / currentP01Delta;
                }
                else
                    currentP1 = currentP01 = frameTimeStamps.Min(x => x.Item1);
            }

            var text = new StringBuilder(27); // optimal size for no extra info

            text.AppendLine($"FPS: {currentAvg:F0}   1% {currentP1:F0}   0.1% {currentP01:F0}");

            foreach (var (key, value) in extraInfo)
                text.AppendLine($"{key}: {value}");

            informationGui.text = text.ToString();
            extraInfoChanged = false;
        }

        public void SetExtraInfo(string key, string value)
        {
            extraInfo[key] = value;
            extraInfoChanged = true;
        }
    }
}
