using MagicARAssistant.UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MagicARAssistant.AR
{
    public sealed class TrackedMarkerController : MonoBehaviour
    {
        private MarkerOverlayView _view;
        private string _markerId;
        private float _lastSeenAt;
        private float _hideDelay = 4f;

        public void Initialize(MarkerOverlayView view, string markerId)
        {
            _view = view;
            _markerId = markerId;
            _lastSeenAt = Time.time;
            _hideDelay = PlayerPrefs.GetFloat("tracking_hide_delay", 4f);
        }

        public void UpdateFromTrackedImage(ARTrackedImage trackedImage)
        {
            bool visible = TrackingStabilityService.IsVisible(trackedImage.trackingState);
            if (visible)
            {
                _lastSeenAt = Time.time;
                transform.position = trackedImage.transform.position + trackedImage.transform.up * 0.02f;
                transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
                gameObject.SetActive(true);
                _view.RefreshMessage();
            }
            else if (Time.time - _lastSeenAt > _hideDelay)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

