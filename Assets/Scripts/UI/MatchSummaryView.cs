using System.Linq;
using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class MatchSummaryView : MonoBehaviour
    {
        private ToastView _toast;

        public void Build()
        {
            Canvas canvas = UIBuilder.CreateScreenCanvas("MatchSummaryCanvas");
            RectTransform background = UIBuilder.CreatePanel(canvas.transform, "Background", UIBuilder.Background);
            UIBuilder.Stretch(background);

            Text title = UIBuilder.CreateText(canvas.transform, "Title", "Resumo da partida", 58, FontStyle.Bold);
            UIBuilder.SetTopLeft(title.GetComponent<RectTransform>(), 42, 42, 1000, 92);

            RectTransform panel = UIBuilder.CreatePanel(canvas.transform, "SummaryPanel", new Color(0.08f, 0.09f, 0.10f, 0.94f));
            UIBuilder.SetTopLeft(panel, 42, 160, 1318, 1740);

            Text summary = UIBuilder.CreateText(panel, "Summary", BuildSummaryText(), 32, FontStyle.Normal, TextAnchor.UpperLeft);
            UIBuilder.Stretch(summary.GetComponent<RectTransform>(), 32, 280, 32, 32);

            Button export = UIBuilder.CreateButton(panel, "Export", "Exportar log", UIBuilder.Accent);
            UIBuilder.SetTopLeft(export.GetComponent<RectTransform>(), 32, 32, 360, 92);
            export.onClick.AddListener(ExportLog);
            Button newMatch = UIBuilder.CreateButton(panel, "NewMatch", "Nova partida", UIBuilder.Warning);
            UIBuilder.SetTopLeft(newMatch.GetComponent<RectTransform>(), 430, 32, 360, 92);
            newMatch.onClick.AddListener(() =>
            {
                AppServices.MatchStateManager.StartNewMatch();
                SceneLoader.LoadARMatch();
            });
            Button menu = UIBuilder.CreateButton(panel, "Menu", "Voltar ao menu", new Color(0.30f, 0.34f, 0.38f, 1f));
            UIBuilder.SetTopLeft(menu.GetComponent<RectTransform>(), 828, 32, 430, 92);
            menu.onClick.AddListener(SceneLoader.LoadMainMenu);

            _toast = ToastView.Create(canvas);
        }

        private string BuildSummaryText()
        {
            var state = AppServices.MatchStateManager.State;
            int lifeChanges = state.eventLog.Count(entry => entry.eventType == "Vida alterada");
            int trackingFailures = state.eventLog.Count(entry => entry.eventType.Contains("Tracking"));
            string cards = state.detectedCards.Count == 0
                ? "Nenhuma carta detectada/adicionada."
                : string.Join("\n", state.detectedCards.Select(card => $"- {card.displayName}: +1/+1 {card.countersPlusOne}, dano {card.damageCounters}, habilidades {card.abilityMarkers.Count}"));

            return $"Vida final\nJogador A: {state.playerALife}\nJogador B: {state.playerBLife}\n\n" +
                   $"Turno final: {state.turnNumber}\nJogador ativo: {state.activePlayer}\nFase: {state.phase}\n\n" +
                   $"Cartas detectadas/adicionadas\n{cards}\n\n" +
                   $"Marcadores aplicados: {state.appliedMarkers.Count}\n" +
                   $"Alteracoes de vida: {lifeChanges}\n" +
                   $"Falhas/alteracoes de tracking registradas: {trackingFailures}\n" +
                   $"Criada em: {state.createdAt}\nAtualizada em: {state.updatedAt}";
        }

        private void ExportLog()
        {
            if (AppServices.Storage.ExportLogJson(AppServices.MatchStateManager.State, out string path, out string error))
            {
                AppServices.Storage.ExportLogText(AppServices.MatchStateManager.State, out _, out _);
                _toast.Show($"Log exportado em: {path}");
            }
            else
            {
                _toast.Show(error);
            }
        }
    }
}

