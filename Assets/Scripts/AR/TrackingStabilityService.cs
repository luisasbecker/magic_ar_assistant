using UnityEngine.XR.ARSubsystems;

namespace MagicARAssistant.AR
{
    public static class TrackingStabilityService
    {
        public static string ToDisplayState(TrackingState state)
        {
            return state switch
            {
                TrackingState.Tracking => "Estavel",
                TrackingState.Limited => "Rastreamento instavel",
                _ => "Tracking perdido"
            };
        }

        public static bool IsVisible(TrackingState state)
        {
            return state == TrackingState.Tracking || state == TrackingState.Limited;
        }

        public static bool IsUnstable(TrackingState state)
        {
            return state == TrackingState.Limited;
        }
    }
}

