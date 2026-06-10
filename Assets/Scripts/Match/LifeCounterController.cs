using MagicARAssistant.Data;

namespace MagicARAssistant.Match
{
    public sealed class LifeCounterController
    {
        private readonly MatchStateManager _stateManager;
        private readonly UndoService _undoService;
        private readonly MatchLogService _logService;

        public LifeCounterController(MatchStateManager stateManager, UndoService undoService, MatchLogService logService)
        {
            _stateManager = stateManager;
            _undoService = undoService;
            _logService = logService;
        }

        public bool ChangeLife(string player, int delta, bool allowBelowZero = false)
        {
            MatchState state = _stateManager.State;
            bool isPlayerA = player == "Jogador A";
            int oldValue = isPlayerA ? state.playerALife : state.playerBLife;
            int newValue = oldValue + delta;
            if (newValue < 0 && !allowBelowZero)
            {
                newValue = 0;
            }

            SetLifeDirect(player, newValue);
            _logService.Add(state, "Vida alterada", $"{player}: {oldValue} para {newValue}", "", oldValue.ToString(), newValue.ToString());
            _undoService.Register($"Desfazer vida de {player}", () =>
            {
                SetLifeDirect(player, oldValue);
                _logService.Add(state, "Acao desfeita", $"Vida de {player} restaurada.", "", newValue.ToString(), oldValue.ToString());
                _stateManager.NotifyStateChanged();
            });
            _stateManager.NotifyStateChanged();
            return true;
        }

        public void ResetLife(string player)
        {
            MatchState state = _stateManager.State;
            bool isPlayerA = player == "Jogador A";
            int oldValue = isPlayerA ? state.playerALife : state.playerBLife;
            SetLifeDirect(player, 20);
            _logService.Add(state, "Vida alterada", $"{player}: reset para 20", "", oldValue.ToString(), "20");
            _undoService.Register($"Desfazer reset de {player}", () =>
            {
                SetLifeDirect(player, oldValue);
                _logService.Add(state, "Acao desfeita", $"Reset de {player} desfeito.", "", "20", oldValue.ToString());
                _stateManager.NotifyStateChanged();
            });
            _stateManager.NotifyStateChanged();
        }

        private void SetLifeDirect(string player, int value)
        {
            if (player == "Jogador A")
            {
                _stateManager.State.playerALife = value;
            }
            else
            {
                _stateManager.State.playerBLife = value;
            }
        }
    }
}

