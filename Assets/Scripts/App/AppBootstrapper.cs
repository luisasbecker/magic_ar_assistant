using UnityEngine;

namespace MagicARAssistant.App
{
    public sealed class AppBootstrapper : MonoBehaviour
    {
        private static AppBootstrapper _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            AppServices.EnsureInitialized();
        }
    }
}

