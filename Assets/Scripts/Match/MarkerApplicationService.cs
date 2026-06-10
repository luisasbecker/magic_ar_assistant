using System.Linq;
using MagicARAssistant.Data;

namespace MagicARAssistant.Match
{
    public sealed class MarkerApplicationService
    {
        private readonly MatchStateManager _stateManager;
        private readonly UndoService _undoService;
        private readonly MatchLogService _logService;

        public MarkerApplicationService(MatchStateManager stateManager, UndoService undoService, MatchLogService logService)
        {
            _stateManager = stateManager;
            _undoService = undoService;
            _logService = logService;
        }

        public bool ApplyMarkerToSelected(MarkerData marker)
        {
            DetectedCardState selected = _stateManager.GetSelectedCard();
            return ApplyMarker(selected, marker);
        }

        public bool ApplyMarker(DetectedCardState card, MarkerData marker)
        {
            if (card == null || marker == null)
            {
                return false;
            }

            string oldValue = Snapshot(card);
            ApplyMarkerDirect(card, marker);
            string markerLog = $"{marker.id}:{card.instanceId}";
            _stateManager.State.appliedMarkers.Add(markerLog);
            _logService.Add(_stateManager.State, "Marcador aplicado", $"{marker.label} aplicado em {card.displayName}", card.instanceId, oldValue, Snapshot(card));
            _undoService.Register($"Desfazer {marker.label}", () =>
            {
                RemoveMarkerDirect(card, marker);
                _stateManager.State.appliedMarkers.Remove(markerLog);
                _logService.Add(_stateManager.State, "Acao desfeita", $"{marker.label} removido de {card.displayName}", card.instanceId, Snapshot(card), oldValue);
                _stateManager.NotifyStateChanged();
            });
            _stateManager.NotifyStateChanged();
            return true;
        }

        public bool RemoveMarker(DetectedCardState card, MarkerData marker)
        {
            if (card == null || marker == null)
            {
                return false;
            }

            string oldValue = Snapshot(card);
            RemoveMarkerDirect(card, marker);
            _logService.Add(_stateManager.State, "Marcador removido", $"{marker.label} removido de {card.displayName}", card.instanceId, oldValue, Snapshot(card));
            _stateManager.NotifyStateChanged();
            return true;
        }

        private static void ApplyMarkerDirect(DetectedCardState card, MarkerData marker)
        {
            switch (marker.markerType)
            {
                case "PLUS_ONE":
                    card.countersPlusOne += marker.value;
                    break;
                case "DAMAGE":
                    card.damageCounters += marker.value;
                    break;
                case "ABILITY":
                    if (!card.abilityMarkers.Contains(marker.label))
                    {
                        card.abilityMarkers.Add(marker.label);
                    }
                    break;
            }
        }

        private static void RemoveMarkerDirect(DetectedCardState card, MarkerData marker)
        {
            switch (marker.markerType)
            {
                case "PLUS_ONE":
                    card.countersPlusOne = System.Math.Max(0, card.countersPlusOne - marker.value);
                    break;
                case "DAMAGE":
                    card.damageCounters = System.Math.Max(0, card.damageCounters - marker.value);
                    break;
                case "ABILITY":
                    string ability = card.abilityMarkers.FirstOrDefault(item => item == marker.label);
                    if (!string.IsNullOrEmpty(ability))
                    {
                        card.abilityMarkers.Remove(ability);
                    }
                    break;
            }
        }

        private static string Snapshot(DetectedCardState card)
        {
            return $"+1/+1={card.countersPlusOne}; dano={card.damageCounters}; habilidades={card.abilityMarkers.Count}";
        }
    }
}

