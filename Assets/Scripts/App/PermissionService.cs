using System;
using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace MagicARAssistant.App
{
    public sealed class PermissionService : MonoBehaviour
    {
        public const string CameraPrivacyMessage =
            "O Magic AR Assistant usa a camera apenas para reconhecer cartas e marcadores em Realidade Aumentada. Nenhuma imagem e enviada para servidores.";

        public bool HasCameraPermission()
        {
#if UNITY_ANDROID
            return Permission.HasUserAuthorizedPermission(Permission.Camera);
#elif UNITY_IOS
            return Application.HasUserAuthorization(UserAuthorization.WebCam);
#else
            return true;
#endif
        }

        public IEnumerator RequestCameraPermission(Action<bool> completed)
        {
            if (HasCameraPermission())
            {
                completed?.Invoke(true);
                yield break;
            }

#if UNITY_ANDROID
            Permission.RequestUserPermission(Permission.Camera);
            float timeoutAt = Time.realtimeSinceStartup + 12f;
            while (!HasCameraPermission() && Time.realtimeSinceStartup < timeoutAt)
            {
                yield return null;
            }
            completed?.Invoke(HasCameraPermission());
#elif UNITY_IOS
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            completed?.Invoke(HasCameraPermission());
#else
            completed?.Invoke(true);
            yield break;
#endif
        }
    }
}

