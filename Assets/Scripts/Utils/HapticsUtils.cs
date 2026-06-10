using UnityEngine;

namespace MagicARAssistant.Utils
{
    public static class HapticsUtils
    {
        public static void LightImpact(bool enabled)
        {
            if (!enabled)
            {
                return;
            }

#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }
}

