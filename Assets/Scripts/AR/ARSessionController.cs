using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MagicARAssistant.AR
{
    public sealed class ARSessionController : MonoBehaviour
    {
        [SerializeField] private ARSession arSession;

        private void Awake()
        {
            if (arSession == null)
            {
                arSession = FindObjectOfType<ARSession>();
            }
        }

        private void OnEnable()
        {
            if (arSession != null)
            {
                arSession.enabled = true;
            }
        }

        public void ResetSession()
        {
            if (arSession != null)
            {
                arSession.Reset();
            }
        }
    }
}

