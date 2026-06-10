using System.Linq;
using MagicARAssistant.App;
using MagicARAssistant.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class ManualModeView : MonoBehaviour
    {
        private RectTransform _cardList;
        private Text _selectedText;
        private LogPanelView _logPanel;
        private ToastView _toast;

        public void Build()
        {
            Canvas canvas = UIBuilder.CreateScreenCanvas("ManualModeCanvas");
            RectTransform background = UIBuilder.CreatePanel(canvas.transform, "Background", UIBuilder.Background);
            UIBuilder.Stretch(background);

            Text title = UIBuilder.CreateText(canvas.transform, "Title", "Modo manual", 58, FontStyle.Bold, TextAnchor.MiddleLeft);
            UIBuilder.SetTopLeft(title.GetComponent<RectTransform>(), 42, 32, 760, 92);

            Button menu = UIBuilder.CreateButton(canvas.transform, "Menu", "Menu", UIBuilder.Warning);
            UIBuilder.SetTopLeft(menu.GetComponent<RectTransform>(), 1120, 42, 240, 82);
            menu.onClick.AddListener(SceneLoader.LoadMainMenu);

            _selectedText = UIBuilder.CreateText(canvas.transform, "Selected", "Nenhuma carta selecionada.", 32, FontStyle.Bold);
            UIBuilder.SetTopLeft(_selectedText.GetComponent<RectTransform>(), 42, 140, 1260, 72);

            _cardList = UIBuilder.CreatePanel(canvas.transform, "CardList", new Color(0.08f, 0.09f, 0.10f, 0.92f));
            UIBuilder.SetTopLeft(_cardList, 42, 230, 650, 1220);
            UIBuilder.AddVerticalLayout(_cardList.gameObject, 16, new RectOffset(18, 18, 18, 18));

            RectTransform actions = UIBuilder.CreatePanel(canvas.transform, "Actions", new Color(0.08f, 0.09f, 0.10f, 0.92f));
            UIBuilder.SetTopLeft(actions, 720, 230, 640, 1220);
            UIBuilder.AddVerticalLayout(actions.gameObject, 18, new RectOffset(18, 18, 18, 18));

            Text actionTitle = UIBuilder.CreateText(actions, "ActionTitle", "Marcadores e partida", 36, FontStyle.Bold, TextAnchor.MiddleCenter);
            actionTitle.gameObject.AddComponent<LayoutElement>().preferredHeight = 70;
            foreach (MarkerData marker in AppServices.DataRepository.Markers)
            {
                Button markerButton = UIBuilder.CreateButton(actions, marker.id, marker.label, UIBuilder.Accent);
                markerButton.gameObject.AddComponent<LayoutElement>().preferredHeight = 86;
                markerButton.onClick.AddListener(() => ApplyMarker(marker));
            }

            AddAction(actions, "Remover +1/+1", UIBuilder.Warning, () => RemoveMarker("MARKER_PLUS_ONE"));
            AddAction(actions, "Remover dano", UIBuilder.Warning, () => RemoveMarker("MARKER_DAMAGE"));
            AddAction(actions, "Salvar partida", new Color(0.36f, 0.48f, 0.82f, 1f), SaveMatch);
            AddAction(actions, "Exportar log", new Color(0.36f, 0.48f, 0.82f, 1f), ExportLog);
            AddAction(actions, "Resumo", UIBuilder.Accent, SceneLoader.LoadSummary);
            AddAction(actions, "Log", UIBuilder.Warning, () => _logPanel.Toggle());

            HUDView.Create(canvas);
            _logPanel = LogPanelView.Create(canvas);
            _toast = ToastView.Create(canvas);
            BuildCardList();
            AppServices.MatchStateManager.StateChanged += _ => RefreshSelected();
            RefreshSelected();
        }

        private void BuildCardList()
        {
            foreach (CardData card in AppServices.DataRepository.Cards)
            {
                Button button = UIBuilder.CreateButton(_cardList, card.id, $"{card.name}\n{card.typeLine}", new Color(0.20f, 0.24f, 0.28f, 1f));
                button.gameObject.AddComponent<LayoutElement>().preferredHeight = 132;
                button.onClick.AddListener(() => AddAndSelectCard(card));
            }
        }

        private void AddAndSelectCard(CardData card)
        {
            string instanceId = "MANUAL_" + card.id + "_" + AppServices.MatchStateManager.State.detectedCards.Count;
            AppServices.MatchStateManager.AddOrUpdateDetectedCard(card, instanceId, "Manual", true);
            AppServices.MatchStateManager.SelectCard(instanceId);
            _toast.Show($"{card.name} adicionada manualmente.");
        }

        private void ApplyMarker(MarkerData marker)
        {
            if (!AppServices.MarkerApplicationService.ApplyMarkerToSelected(marker))
            {
                _toast.Show("Selecione uma carta antes de aplicar o marcador.");
            }
        }

        private void RemoveMarker(string markerId)
        {
            DetectedCardState selected = AppServices.MatchStateManager.GetSelectedCard();
            MarkerData marker = AppServices.DataRepository.GetMarkerById(markerId);
            if (selected == null || marker == null)
            {
                _toast.Show("Selecione uma carta antes de remover marcador.");
                return;
            }

            AppServices.MarkerApplicationService.RemoveMarker(selected, marker);
        }

        private void SaveMatch()
        {
            if (AppServices.Storage.SaveMatch(AppServices.MatchStateManager.State, out string error))
            {
                AppServices.LogService.Add(AppServices.MatchStateManager.State, "Partida salva", "Partida salva localmente.");
                _toast.Show("Partida salva.");
            }
            else
            {
                _toast.Show(error);
            }
        }

        private void ExportLog()
        {
            if (AppServices.Storage.ExportLogJson(AppServices.MatchStateManager.State, out string path, out string error))
            {
                AppServices.Storage.ExportLogText(AppServices.MatchStateManager.State, out _, out _);
                AppServices.LogService.Add(AppServices.MatchStateManager.State, "Partida finalizada", "Log exportado manualmente.");
                _toast.Show($"Log exportado em: {path}");
            }
            else
            {
                _toast.Show(error);
            }
        }

        private void RefreshSelected()
        {
            DetectedCardState selected = AppServices.MatchStateManager.GetSelectedCard();
            if (selected == null)
            {
                _selectedText.text = "Nenhuma carta selecionada.";
                return;
            }

            string abilities = selected.abilityMarkers.Count == 0 ? "nenhuma" : string.Join(", ", selected.abilityMarkers);
            _selectedText.text = $"{selected.displayName} | +1/+1: {selected.countersPlusOne} | Dano: {selected.damageCounters} | Habilidades: {abilities}";
        }

        private static void AddAction(Transform parent, string label, Color color, UnityEngine.Events.UnityAction action)
        {
            Button button = UIBuilder.CreateButton(parent, label, label, color);
            button.gameObject.AddComponent<LayoutElement>().preferredHeight = 82;
            button.onClick.AddListener(action);
        }
    }
}

