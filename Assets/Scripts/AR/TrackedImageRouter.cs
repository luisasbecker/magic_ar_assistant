using MagicARAssistant.App;
using MagicARAssistant.Data;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MagicARAssistant.AR
{
    public sealed class TrackedImageRouter : MonoBehaviour
    {
        [SerializeField] private AROverlayFactory overlayFactory;

        public void SetFactory(AROverlayFactory factory)
        {
            overlayFactory = factory;
        }

        public bool TryRoute(ARTrackedImage trackedImage, out TrackedCardController cardController, out TrackedMarkerController markerController)
        {
            cardController = null;
            markerController = null;
            string imageName = trackedImage.referenceImage.name;
            CardData card = AppServices.DataRepository.GetCardByReferenceImage(imageName);
            if (card != null)
            {
                string instanceId = trackedImage.trackableId.ToString();
                string displayState = TrackingStabilityService.ToDisplayState(trackedImage.trackingState);
                DetectedCardState state = AppServices.MatchStateManager.AddOrUpdateDetectedCard(card, instanceId, displayState, true);
                cardController = overlayFactory.CreateCardOverlay(card, state, trackedImage.transform);
                return true;
            }

            MarkerData marker = AppServices.DataRepository.GetMarkerByReferenceImage(imageName);
            if (marker != null)
            {
                AppServices.LogService.Add(AppServices.MatchStateManager.State, "Marcador detectado", $"Marcador detectado: {marker.label}");
                markerController = overlayFactory.CreateMarkerOverlay(marker, trackedImage.transform);
                return true;
            }

            return false;
        }
    }
}

