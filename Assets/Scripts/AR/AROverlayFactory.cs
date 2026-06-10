using MagicARAssistant.Data;
using MagicARAssistant.UI;
using UnityEngine;

namespace MagicARAssistant.AR
{
    public sealed class AROverlayFactory : MonoBehaviour
    {
        public TrackedCardController CreateCardOverlay(CardData cardData, DetectedCardState state, Transform trackedTransform)
        {
            GameObject overlay = new($"CardOverlay_{cardData.id}");
            overlay.transform.position = trackedTransform.position;
            overlay.transform.rotation = trackedTransform.rotation;
            CardOverlayView view = overlay.AddComponent<CardOverlayView>();
            view.Build(cardData, state);
            TrackedCardController controller = overlay.AddComponent<TrackedCardController>();
            controller.Initialize(view, state.instanceId);
            BoxCollider collider = overlay.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.75f, 0.52f, 0.04f);
            collider.center = Vector3.zero;
            return controller;
        }

        public TrackedMarkerController CreateMarkerOverlay(MarkerData markerData, Transform trackedTransform)
        {
            GameObject overlay = new($"MarkerOverlay_{markerData.id}");
            overlay.transform.position = trackedTransform.position;
            overlay.transform.rotation = trackedTransform.rotation;
            MarkerOverlayView view = overlay.AddComponent<MarkerOverlayView>();
            view.Build(markerData);
            TrackedMarkerController controller = overlay.AddComponent<TrackedMarkerController>();
            controller.Initialize(view, markerData.id);
            return controller;
        }
    }
}

