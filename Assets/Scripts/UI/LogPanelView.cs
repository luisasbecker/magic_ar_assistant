using System.Linq;
using MagicARAssistant.App;
using MagicARAssistant.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class LogPanelView : MonoBehaviour
    {
        private RectTransform _panel;
        private Text _content;
        private bool _visible;

        public static LogPanelView Create(Canvas canvas)
        {
            RectTransform panel = UIBuilder.CreatePanel(canvas.transform, "LogPanel", new Color(0.03f, 0.04f, 0.05f, 0.94f));
            UIBuilder.SetTopLeft(panel, 40, 240, 1320, 1180);
            panel.gameObject.SetActive(false);

            Text title = UIBuilder.CreateText(panel, "Title", "Log da partida", 42, FontStyle.Bold);
            UIBuilder.SetTopLeft(title.GetComponent<RectTransform>(), 28, 24, 760, 70);
            Button close = UIBuilder.CreateButton(panel, "CloseButton", "Fechar", UIBuilder.Warning);
            UIBuilder.SetTopLeft(close.GetComponent<RectTransform>(), 1040, 24, 240, 72);

            RectTransform scrollRectTransform = UIBuilder.CreatePanel(panel, "ScrollArea", new Color(0f, 0f, 0f, 0.25f));
            UIBuilder.SetTopLeft(scrollRectTransform, 28, 120, 1260, 1010);
            ScrollRect scroll = scrollRectTransform.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false;

            RectTransform viewport = UIBuilder.CreatePanel(scrollRectTransform, "Viewport", new Color(0f, 0f, 0f, 0f));
            UIBuilder.Stretch(viewport);
            Mask mask = viewport.gameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;
            scroll.viewport = viewport;

            GameObject contentObject = new("Content");
            contentObject.transform.SetParent(viewport, false);
            RectTransform contentRect = contentObject.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.sizeDelta = new Vector2(0, 1800);
            Text content = UIBuilder.CreateText(contentObject.transform, "ContentText", "", 28, FontStyle.Normal, TextAnchor.UpperLeft);
            UIBuilder.Stretch(content.GetComponent<RectTransform>(), 20, 20, 20, 20);
            scroll.content = contentRect;

            LogPanelView view = panel.gameObject.AddComponent<LogPanelView>();
            view._panel = panel;
            view._content = content;
            close.onClick.AddListener(view.Hide);
            return view;
        }

        public void Toggle()
        {
            if (_visible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show()
        {
            _visible = true;
            _panel.gameObject.SetActive(true);
            Refresh(AppServices.MatchStateManager.State);
        }

        public void Hide()
        {
            _visible = false;
            _panel.gameObject.SetActive(false);
        }

        public void Refresh(MatchState state)
        {
            if (state == null || _content == null)
            {
                return;
            }

            int skip = Mathf.Max(0, state.eventLog.Count - 120);
            _content.text = string.Join("\n", state.eventLog.Skip(skip).Select(entry => $"{entry.timestamp} | {entry.eventType} | {entry.description}"));
        }
    }
}
