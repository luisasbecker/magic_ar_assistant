using MagicARAssistant.App;
using MagicARAssistant.Data;
using MagicARAssistant.UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace MagicARAssistant.AR
{
    public sealed class TrackedCardController : MonoBehaviour
    {
        private CardOverlayView _view;
        private string _instanceId;
        private float _lastSeenAt;
        private float _hideDelay = 4f;

        public string InstanceId => _instanceId;

        public void Initialize(CardOverlayView view, string instanceId)
        {
            _view = view;
            _instanceId = instanceId;
            _lastSeenAt = Time.time;
            _hideDelay = PlayerPrefs.GetFloat("tracking_hide_delay", 4f);
        }

        public void UpdateFromTrackedImage(ARTrackedImage trackedImage)
        {
            bool visible = TrackingStabilityService.IsVisible(trackedImage.trackingState);
            if (visible)
            {
                _lastSeenAt = Time.time;
                transform.position = trackedImage.transform.position + trackedImage.transform.up * 0.015f;
                transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
                gameObject.SetActive(true);
            }
            else if (Time.time - _lastSeenAt > _hideDelay)
            {
                DetectedCardState card = AppServices.MatchStateManager.GetCardByInstanceId(_instanceId);
                if (card == null || !card.isPinned)
                {
                    gameObject.SetActive(false);
                }
            }

            DetectedCardState state = AppServices.MatchStateManager.GetCardByInstanceId(_instanceId);
            _view.Refresh(state);
            _view.SetTrackingUnstable(trackedImage.trackingState == TrackingState.Limited);
        }
    }
}

