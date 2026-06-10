using MagicARAssistant.Data;
using MagicARAssistant.Match;

namespace MagicARAssistant.App
{
    public static class AppServices
    {
        public static bool IsInitialized { get; private set; }
        public static DataRepository DataRepository { get; private set; }
        public static LocalJsonStorage Storage { get; private set; }
        public static MatchLogService LogService { get; private set; }
        public static UndoService UndoService { get; private set; }
        public static MatchStateManager MatchStateManager { get; private set; }
        public static LifeCounterController LifeCounterController { get; private set; }
        public static TurnPhaseController TurnPhaseController { get; private set; }
        public static MarkerApplicationService MarkerApplicationService { get; private set; }

        public static void EnsureInitialized()
        {
            if (IsInitialized)
            {
                return;
            }

            DataRepository = new DataRepository();
            DataRepository.Load();
            Storage = new LocalJsonStorage();
            UndoService = new UndoService();
            LogService = new MatchLogService();
            MatchStateManager = new MatchStateManager(LogService);
            LifeCounterController = new LifeCounterController(MatchStateManager, UndoService, LogService);
            TurnPhaseController = new TurnPhaseController(MatchStateManager, UndoService, LogService);
            MarkerApplicationService = new MarkerApplicationService(MatchStateManager, UndoService, LogService);
            MatchStateManager.StartNewMatch();
            IsInitialized = true;
        }
    }
}

