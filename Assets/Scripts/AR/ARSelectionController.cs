using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagicARAssistant.AR
{
    public sealed class ARSelectionController : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.touchCount == 0)
            {
                return;
            }

            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Ended)
            {
                return;
            }

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                TrackedCardController card = hit.collider.GetComponentInParent<TrackedCardController>();
                if (card != null)
                {
                    AppServices.MatchStateManager.SelectCard(card.InstanceId);
                }
            }
        }
    }
}

