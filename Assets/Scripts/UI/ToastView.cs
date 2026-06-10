using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class ToastView : MonoBehaviour
    {
        private Text _text;
        private CanvasGroup _group;
        private float _hideAt;

        public static ToastView Create(Canvas canvas)
        {
            RectTransform panel = UIBuilder.CreatePanel(canvas.transform, "Toast", new Color(0.02f, 0.02f, 0.02f, 0.86f));
            UIBuilder.SetBottom(panel, 0, 92, 1120, 120);
            CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0;
            Text text = UIBuilder.CreateText(panel, "ToastText", "", 30, FontStyle.Normal, TextAnchor.MiddleCenter);
            UIBuilder.Stretch(text.GetComponent<RectTransform>(), 24, 12, 24, 12);
            ToastView view = panel.gameObject.AddComponent<ToastView>();
            view._text = text;
            view._group = group;
            return view;
        }

        public void Show(string message, float seconds = 3f)
        {
            _text.text = message;
            _group.alpha = 1;
            _hideAt = Time.unscaledTime + seconds;
        }

        private void Update()
        {
            if (_group.alpha > 0 && Time.unscaledTime > _hideAt)
            {
                _group.alpha = Mathf.MoveTowards(_group.alpha, 0, Time.unscaledDeltaTime * 4f);
            }
        }
    }
}

