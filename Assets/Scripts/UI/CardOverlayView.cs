using System.Linq;
using MagicARAssistant.App;
using MagicARAssistant.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class CardOverlayView : MonoBehaviour
    {
        private Text _title;
        private Text _body;
        private Image _background;
        private string _instanceId;
        private CardData _cardData;

        public void Build(CardData cardData, DetectedCardState state)
        {
            _cardData = cardData;
            _instanceId = state.instanceId;
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            gameObject.AddComponent<GraphicRaycaster>();
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(620, 430);
            rect.localScale = Vector3.one * 0.0018f;

            _background = gameObject.AddComponent<Image>();
            _background.color = new Color(0.04f, 0.05f, 0.06f, 0.82f);
            _title = UIBuilder.CreateText(transform, "Title", "", 36, FontStyle.Bold, TextAnchor.UpperLeft);
            UIBuilder.SetTopLeft(_title.GetComponent<RectTransform>(), 18, 14, 584, 58);
            _body = UIBuilder.CreateText(transform, "Body", "", 24, FontStyle.Normal, TextAnchor.UpperLeft);
            UIBuilder.SetTopLeft(_body.GetComponent<RectTransform>(), 18, 76, 584, 194);

            string[] labels = { "+1/+1", "-1/-1", "+ Dano", "- Dano", "Habilidade", "Fixar", "Ocultar" };
            for (int i = 0; i < labels.Length; i++)
            {
                Button button = UIBuilder.CreateButton(transform, labels[i], labels[i], i < 5 ? UIBuilder.Accent : UIBuilder.Warning);
                UIBuilder.SetTopLeft(button.GetComponent<RectTransform>(), 18 + (i % 3) * 196, 282 + (i / 3) * 66, 184, 56);
                string label = labels[i];
                button.onClick.AddListener(() => HandleAction(label));
            }

            Button select = UIBuilder.CreateButton(transform, "Select", "Selecionar", new Color(0.36f, 0.48f, 0.82f, 1f));
            UIBuilder.SetTopLeft(select.GetComponent<RectTransform>(), 410, 348, 190, 56);
            select.onClick.AddListener(() => AppServices.MatchStateManager.SelectCard(_instanceId));
            Refresh(state);
        }

        public void Refresh(DetectedCardState state)
        {
            if (state == null || _cardData == null)
            {
                return;
            }

            bool selected = AppServices.MatchStateManager.SelectedCardInstanceId == _instanceId;
            _background.color = selected ? new Color(0.11f, 0.23f, 0.20f, 0.92f) : new Color(0.04f, 0.05f, 0.06f, 0.82f);
            string abilities = state.abilityMarkers.Count == 0 ? "nenhuma" : string.Join(", ", state.abilityMarkers);
            _title.text = _cardData.name;
            _body.text = $"{_cardData.typeLine} | Custo: {_cardData.manaCost}\n{_cardData.rulesText}\nP/R: {_cardData.power}/{_cardData.toughness}\n+1/+1: {state.countersPlusOne} | Dano: {state.damageCounters}\nHabilidades: {abilities}\nTracking: {state.lastTrackingState}";
        }

        public void SetTrackingUnstable(bool unstable)
        {
            if (_body == null)
            {
                return;
            }

            if (unstable && !_body.text.Contains("Rastreamento instavel"))
            {
                _body.text += "\nRastreamento instavel";
            }
        }

        private void HandleAction(string label)
        {
            DetectedCardState card = AppServices.MatchStateManager.GetCardByInstanceId(_instanceId);
            if (card == null)
            {
                return;
            }

            switch (label)
            {
                case "+1/+1":
                    AppServices.MarkerApplicationService.ApplyMarker(card, AppServices.DataRepository.GetMarkerById("MARKER_PLUS_ONE"));
                    break;
                case "-1/-1":
                    AppServices.MarkerApplicationService.RemoveMarker(card, AppServices.DataRepository.GetMarkerById("MARKER_PLUS_ONE"));
                    break;
                case "+ Dano":
                    AppServices.MarkerApplicationService.ApplyMarker(card, AppServices.DataRepository.GetMarkerById("MARKER_DAMAGE"));
                    break;
                case "- Dano":
                    AppServices.MarkerApplicationService.RemoveMarker(card, AppServices.DataRepository.GetMarkerById("MARKER_DAMAGE"));
                    break;
                case "Habilidade":
                    AppServices.MarkerApplicationService.ApplyMarker(card, AppServices.DataRepository.GetMarkerById("MARKER_ABILITY"));
                    break;
                case "Fixar":
                    card.isPinned = !card.isPinned;
                    AppServices.MatchStateManager.NotifyStateChanged();
                    break;
                case "Ocultar":
                    card.isVisible = false;
                    gameObject.SetActive(false);
                    AppServices.MatchStateManager.NotifyStateChanged();
                    break;
            }

            Refresh(card);
        }
    }
}

