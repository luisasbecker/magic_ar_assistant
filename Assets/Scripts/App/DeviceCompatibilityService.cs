using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace MagicARAssistant.App
{
    public sealed class DeviceCompatibilityService : MonoBehaviour
    {
        public string BuildCompatibilitySummary()
        {
            string platform = Application.platform.ToString();
            string device = SystemInfo.deviceModel;
            string graphics = SystemInfo.graphicsDeviceName;
            string arAvailability = ARSession.state.ToString();
            return $"Plataforma: {platform}\nDispositivo: {device}\nGPU: {graphics}\nEstado AR: {arAvailability}\nImage Tracking: ARCore/ARKit via AR Foundation";
        }

        public bool IsTrackingStateUsable(TrackingState state)
        {
            return state == TrackingState.Tracking || state == TrackingState.Limited;
        }
    }
}

