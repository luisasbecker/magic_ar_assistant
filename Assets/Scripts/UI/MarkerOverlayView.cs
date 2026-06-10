using MagicARAssistant.App;
using MagicARAssistant.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class MarkerOverlayView : MonoBehaviour
    {
        private MarkerData _markerData;
        private Text _body;

        public void Build(MarkerData markerData)
        {
            _markerData = markerData;
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            gameObject.AddComponent<GraphicRaycaster>();
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(560, 270);
            rect.localScale = Vector3.one * 0.0018f;
            gameObject.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.07f, 0.84f);

            Text title = UIBuilder.CreateText(transform, "Title", markerData.label, 34, FontStyle.Bold);
            UIBuilder.SetTopLeft(title.GetComponent<RectTransform>(), 18, 14, 520, 54);
            _body = UIBuilder.CreateText(transform, "Body", "", 24, FontStyle.Normal, TextAnchor.UpperLeft);
            UIBuilder.SetTopLeft(_body.GetComponent<RectTransform>(), 18, 70, 520, 82);
            RefreshMessage();

            Button apply = UIBuilder.CreateButton(transform, "Apply", "Aplicar a carta selecionada", UIBuilder.Accent);
            UIBuilder.SetTopLeft(apply.GetComponent<RectTransform>(), 18, 166, 350, 68);
            apply.onClick.AddListener(Apply);
            Button cancel = UIBuilder.CreateButton(transform, "Cancel", "Cancelar", UIBuilder.Warning);
            UIBuilder.SetTopLeft(cancel.GetComponent<RectTransform>(), 382, 166, 156, 68);
            cancel.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void RefreshMessage()
        {
            bool hasSelection = AppServices.MatchStateManager.GetSelectedCard() != null;
            _body.text = hasSelection
                ? $"{_markerData.effectDescription}\nValor: {_markerData.value}"
                : "Toque em uma carta detectada para aplicar este marcador.";
        }

        private void Apply()
        {
            if (!AppServices.MarkerApplicationService.ApplyMarkerToSelected(_markerData))
            {
                RefreshMessage();
            }
        }
    }
}

