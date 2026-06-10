using System;
using System.Linq;
using MagicARAssistant.Data;
using MagicARAssistant.Utils;

namespace MagicARAssistant.Match
{
    public sealed class MatchStateManager
    {
        public event Action<MatchState> StateChanged;
        public event Action<DetectedCardState> CardSelected;

        public MatchState State { get; private set; }
        public string SelectedCardInstanceId { get; private set; }

        private readonly MatchLogService _logService;

        public MatchStateManager(MatchLogService logService)
        {
            _logService = logService;
        }

        public void StartNewMatch()
        {
            string now = DateTimeUtils.NowIsoUtc();
            State = new MatchState
            {
                playerALife = 20,
                playerBLife = 20,
                turnNumber = 1,
                activePlayer = "Jogador A",
                phase = "Inicio",
                createdAt = now,
                updatedAt = now
            };
            _logService.Add(State, "Partida iniciada", "Nova partida iniciada.");
            NotifyStateChanged();
        }

        public void SetState(MatchState loadedState)
        {
            State = loadedState ?? throw new ArgumentNullException(nameof(loadedState));
            SelectedCardInstanceId = State.detectedCards.FirstOrDefault()?.instanceId ?? string.Empty;
            _logService.Add(State, "Partida carregada", "Ultima partida carregada do armazenamento local.");
            NotifyStateChanged();
        }

        public DetectedCardState AddOrUpdateDetectedCard(CardData cardData, string instanceId, string trackingState, bool visible)
        {
            if (cardData == null || string.IsNullOrWhiteSpace(instanceId))
            {
                return null;
            }

            DetectedCardState card = State.detectedCards.FirstOrDefault(item => item.instanceId == instanceId);
            bool isNew = card == null;
            if (isNew)
            {
                card = new DetectedCardState
                {
                    instanceId = instanceId,
                    cardId = cardData.id,
                    displayName = cardData.name,
                    countersPlusOne = cardData.defaultCounters,
                    damageCounters = 0,
                    isVisible = visible,
                    lastTrackingState = trackingState
                };
                State.detectedCards.Add(card);
                _logService.Add(State, "Carta detectada", $"Carta detectada: {card.displayName}", card.instanceId);
            }
            else
            {
                string oldTracking = card.lastTrackingState;
                card.isVisible = visible || card.isPinned;
                card.lastTrackingState = trackingState;
                if (oldTracking != trackingState)
                {
                    _logService.Add(State, "Tracking atualizado", $"Tracking de {card.displayName}: {trackingState}", card.instanceId, oldTracking, trackingState);
                }
            }

            NotifyStateChanged();
            return card;
        }

        public bool SelectCard(string instanceId)
        {
            DetectedCardState card = State.detectedCards.FirstOrDefault(item => item.instanceId == instanceId);
            if (card == null)
            {
                return false;
            }

            SelectedCardInstanceId = instanceId;
            _logService.Add(State, "Carta selecionada", $"Carta selecionada: {card.displayName}", card.instanceId);
            CardSelected?.Invoke(card);
            NotifyStateChanged();
            return true;
        }

        public DetectedCardState GetSelectedCard()
        {
            return State.detectedCards.FirstOrDefault(item => item.instanceId == SelectedCardInstanceId);
        }

        public DetectedCardState GetCardByInstanceId(string instanceId)
        {
            return State.detectedCards.FirstOrDefault(item => item.instanceId == instanceId);
        }

        public void NotifyStateChanged()
        {
            State.updatedAt = DateTimeUtils.NowIsoUtc();
            StateChanged?.Invoke(State);
        }
    }
}

