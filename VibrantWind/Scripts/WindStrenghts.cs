// Project:         Vibrant Wind for Daggerfall Unity
// Web Site:        -
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/TheLacus/vibrantwind-du-mod
// Original Author: TheLacus
// Contributors:    

// #define TEST_VALUES

using System;
using UnityEngine;
using DaggerfallWorkshop.Utility;

namespace VibrantWind
{
    /// <summary>
    /// Strenght of wind.
    /// </summary>
    public class WindStrenght
    {
        public const uint Items = 6;

        public float

            None,
            VeryLight,
            Light,
            Medium,
            Strong,
            VeryStrong;
    }

    public struct Interpolation
    {
        public const int

            Lerp = 0,
            Sinerp = 1,
            Coserp = 2,
            SmoothStep = 3;
    }

    public static class WindStrenghts
    {
        /// <summary>
        /// Get all strenght values.
        /// </summary>
        /// <param name="range">Min and max value.</param>
        /// <param name="interpolation">Interpolation to use.</param>
        public static WindStrenght GetStrenghts(Tuple<float, float> range, int interpolation)
        {
            const uint times = WindStrenght.Items - 1;
            var sV = new ScaledValues(range.First, range.Second, times, interpolation);
            return new WindStrenght
            {
                None = sV.NextValue(),
                VeryLight = sV.NextValue(),
                Light = sV.NextValue(),
                Medium = sV.NextValue(),
                Strong = sV.NextValue(),
                VeryStrong = sV.NextValue()
            };
        }

        /// <summary>
        /// Scales a group of values between min and max.
        /// </summary>
        public class ScaledValues
        {
            float min, max;
            uint times, current = 0;
            int interpolation;

            /// <summary>
            /// Get values between min and max using interpolation.
            /// </summary>
            /// <param name="min">0 on 0-1</param>
            /// <param name="max">1 on 0-1</param>
            /// <param name="times">Total number of values.</param>
            /// <param name="interpolation">Type of interpolation to use.</param>
            public ScaledValues(float min, float max, uint times, int interpolation)
            {
                this.min = min;
                this.max = max;
                this.times = times;
                this.interpolation = interpolation;

#if TEST_VALUES
                Debug.Log(string.Format("VibrantWind: interpolation {0}", interpolation));
#endif
            }

            public float NextValue()
            {
                float t = (current != 0) ? (current / (float)times) : 0;
                float value = (float)Math.Round(GetValue(t), 2);

#if TEST_VALUES
                Debug.Log(string.Format("VibrantWind: item {0}, rel {1}, value {2}", current, t, value));
#endif

                current++;
                return value;
            }

            private float GetValue(float t)
            {
                switch (interpolation)
                {
                    case Interpolation.Lerp:
                    default:
                        return Mathf.Lerp(min, max, t);

                    case Interpolation.Sinerp:
                        t = Mathf.Sin(t * Mathf.PI * 0.5f);
                        return Mathf.Lerp(min, max, t);

                    case Interpolation.Coserp:
                        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                        return Mathf.Lerp(min, max, t);

                    case Interpolation.SmoothStep:
                        return Mathf.SmoothStep(min, max, t);
                }
            }
        }
    }
}
