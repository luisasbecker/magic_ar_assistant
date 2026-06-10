using MagicARAssistant.AR;
using MagicARAssistant.UI;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace MagicARAssistant.App
{
    public sealed class SceneAutoInstaller : MonoBehaviour
    {
        private void Awake()
        {
            EnsureBootstrapper();
            EnsureEventSystem();
            InstallForActiveScene();
        }

        private static void EnsureBootstrapper()
        {
            if (FindObjectOfType<AppBootstrapper>() == null)
            {
                new GameObject("AppBootstrapper").AddComponent<AppBootstrapper>();
            }
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            GameObject eventSystem = new("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        private void InstallForActiveScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case SceneLoader.ARMatchScene:
                    InstallARMatchScene();
                    break;
                case SceneLoader.ManualModeScene:
                    gameObject.AddComponent<ManualModeView>().Build();
                    break;
                case SceneLoader.SettingsScene:
                    gameObject.AddComponent<SettingsView>().Build();
                    break;
                case SceneLoader.MatchSummaryScene:
                    gameObject.AddComponent<MatchSummaryView>().Build();
                    break;
                default:
                    gameObject.AddComponent<MainMenuView>().Build();
                    break;
            }
        }

        private void InstallARMatchScene()
        {
            GameObject arSessionObject = new("AR Session");
            ARSession arSession = arSessionObject.AddComponent<ARSession>();
            arSessionObject.AddComponent<ARSessionController>();

            GameObject originObject = new("XR Origin");
            XROrigin origin = originObject.AddComponent<XROrigin>();

            GameObject cameraObject = new("AR Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(originObject.transform, false);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 40f;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.AddComponent<ARCameraManager>();
            cameraObject.AddComponent<ARCameraBackground>();
            origin.Camera = camera;

            ARTrackedImageManager imageManager = originObject.AddComponent<ARTrackedImageManager>();
            imageManager.requestedMaxNumberOfMovingImages = 8;
            originObject.AddComponent<ARRaycastManager>();

            AROverlayFactory factory = gameObject.AddComponent<AROverlayFactory>();
            TrackedImageRouter router = gameObject.AddComponent<TrackedImageRouter>();
            router.SetFactory(factory);
            originObject.AddComponent<ImageTrackingController>();
            gameObject.AddComponent<ARSelectionController>();

            Canvas hudCanvas = UIBuilder.CreateScreenCanvas("AR HUD Canvas");
            HUDView.Create(hudCanvas);
        }
    }
}

