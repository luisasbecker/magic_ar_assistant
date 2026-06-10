using System;
using MagicARAssistant.Data;

namespace MagicARAssistant.Match
{
    public sealed class TurnPhaseController
    {
        public static readonly string[] Phases = { "Inicio", "Principal 1", "Combate", "Principal 2", "Final" };

        private readonly MatchStateManager _stateManager;
        private readonly UndoService _undoService;
        private readonly MatchLogService _logService;

        public TurnPhaseController(MatchStateManager stateManager, UndoService undoService, MatchLogService logService)
        {
            _stateManager = stateManager;
            _undoService = undoService;
            _logService = logService;
        }

        public void NextPhase()
        {
            MatchState state = _stateManager.State;
            string oldPhase = state.phase;
            int index = Array.IndexOf(Phases, state.phase);
            int nextIndex = index < 0 ? 0 : Math.Min(index + 1, Phases.Length - 1);
            state.phase = Phases[nextIndex];
            _logService.Add(state, "Fase alterada", $"Fase: {oldPhase} para {state.phase}", "", oldPhase, state.phase);
            _undoService.Register("Desfazer fase", () =>
            {
                state.phase = oldPhase;
                _logService.Add(state, "Acao desfeita", "Fase restaurada.", "", Phases[nextIndex], oldPhase);
                _stateManager.NotifyStateChanged();
            });
            _stateManager.NotifyStateChanged();
        }

        public void PassTurn()
        {
            MatchState state = _stateManager.State;
            string oldPlayer = state.activePlayer;
            string oldPhase = state.phase;
            int oldTurn = state.turnNumber;
            state.activePlayer = state.activePlayer == "Jogador A" ? "Jogador B" : "Jogador A";
            if (state.activePlayer == "Jogador A")
            {
                state.turnNumber++;
            }

            state.phase = Phases[0];
            _logService.Add(state, "Turno alterado", $"Turno {oldTurn} para {state.turnNumber}; ativo: {state.activePlayer}", "", oldPlayer, state.activePlayer);
            _undoService.Register("Desfazer turno", () =>
            {
                state.activePlayer = oldPlayer;
                state.phase = oldPhase;
                state.turnNumber = oldTurn;
                _logService.Add(state, "Acao desfeita", "Turno restaurado.", "", state.activePlayer, oldPlayer);
                _stateManager.NotifyStateChanged();
            });
            _stateManager.NotifyStateChanged();
        }
    }
}

