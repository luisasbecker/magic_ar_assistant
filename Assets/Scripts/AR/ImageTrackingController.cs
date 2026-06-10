using System.Collections.Generic;
using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace MagicARAssistant.AR
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public sealed class ImageTrackingController : MonoBehaviour
    {
        private readonly Dictionary<string, TrackedCardController> _cardOverlays = new();
        private readonly Dictionary<string, TrackedMarkerController> _markerOverlays = new();

        private ARTrackedImageManager _trackedImageManager;
        private TrackedImageRouter _router;

        private void Awake()
        {
            _trackedImageManager = GetComponent<ARTrackedImageManager>();
            _router = FindObjectOfType<TrackedImageRouter>();
        }

        private void OnEnable()
        {
            if (_trackedImageManager != null)
            {
                _trackedImageManager.trackablesChanged += OnTrackablesChanged;
            }
        }

        private void Start()
        {
            TryCreateRuntimeReferenceLibrary();
        }

        private void OnDisable()
        {
            if (_trackedImageManager != null)
            {
                _trackedImageManager.trackablesChanged -= OnTrackablesChanged;
            }
        }

        private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
        {
            foreach (ARTrackedImage trackedImage in args.added)
            {
                AddOrUpdate(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in args.updated)
            {
                AddOrUpdate(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in args.removed)
            {
                string id = trackedImage.trackableId.ToString();
                if (_cardOverlays.TryGetValue(id, out TrackedCardController card))
                {
                    AppServices.LogService.Add(AppServices.MatchStateManager.State, "Tracking perdido", $"Tracking perdido: {id}", id);
                    card.gameObject.SetActive(false);
                }

                if (_markerOverlays.TryGetValue(id, out TrackedMarkerController marker))
                {
                    marker.gameObject.SetActive(false);
                }
            }
        }

        private void AddOrUpdate(ARTrackedImage trackedImage)
        {
            string id = trackedImage.trackableId.ToString();
            if (_cardOverlays.TryGetValue(id, out TrackedCardController existingCard))
            {
                existingCard.UpdateFromTrackedImage(trackedImage);
                return;
            }

            if (_markerOverlays.TryGetValue(id, out TrackedMarkerController existingMarker))
            {
                existingMarker.UpdateFromTrackedImage(trackedImage);
                return;
            }

            if (_router != null && _router.TryRoute(trackedImage, out TrackedCardController card, out TrackedMarkerController marker))
            {
                if (card != null)
                {
                    _cardOverlays[id] = card;
                    card.UpdateFromTrackedImage(trackedImage);
                }
                else if (marker != null)
                {
                    _markerOverlays[id] = marker;
                    marker.UpdateFromTrackedImage(trackedImage);
                }
            }
        }

        private void TryCreateRuntimeReferenceLibrary()
        {
            if (_trackedImageManager == null || _trackedImageManager.referenceLibrary != null)
            {
                return;
            }

            Texture2D[] textures = Resources.LoadAll<Texture2D>("ReferenceImages");
            if (textures == null || textures.Length == 0)
            {
                Debug.LogWarning("Nenhuma imagem placeholder encontrada em Resources/ReferenceImages. Crie/importe os alvos antes do teste AR.");
                return;
            }

            RuntimeReferenceImageLibrary runtimeLibrary = _trackedImageManager.CreateRuntimeLibrary();
            if (runtimeLibrary is not MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                _trackedImageManager.referenceLibrary = runtimeLibrary;
                Debug.LogWarning("O provedor AR atual nao suporta biblioteca mutavel de imagens em runtime.");
                return;
            }

            foreach (Texture2D texture in textures)
            {
                mutableLibrary.ScheduleAddImageWithValidationJob(texture, texture.name, 0.09f);
            }

            _trackedImageManager.referenceLibrary = mutableLibrary;
            Debug.Log($"Biblioteca runtime de image tracking preparada com {textures.Length} imagens placeholder.");
        }
    }
}
