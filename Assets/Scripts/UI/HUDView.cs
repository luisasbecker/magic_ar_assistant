using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class HUDView : MonoBehaviour
    {
        private LifeCounterView _playerA;
        private LifeCounterView _playerB;
        private TurnPhaseView _turnPhase;
        private LogPanelView _logPanel;
        private ToastView _toast;

        public static HUDView Create(Canvas canvas)
        {
            RectTransform topRow = UIBuilder.CreatePanel(canvas.transform, "HUDTopRow", new Color(0, 0, 0, 0));
            UIBuilder.SetTopLeft(topRow, 28, 28, 1384, 300);
            HorizontalLayoutGroup row = topRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 18;
            row.childControlHeight = true;
            row.childControlWidth = true;
            row.childForceExpandHeight = true;
            row.childForceExpandWidth = true;

            LifeCounterView playerA = LifeCounterView.Create(topRow, "Jogador A");
            LifeCounterView playerB = LifeCounterView.Create(topRow, "Jogador B");

            RectTransform bottomRow = UIBuilder.CreatePanel(canvas.transform, "HUDBottomRow", new Color(0, 0, 0, 0));
            UIBuilder.SetBottom(bottomRow, 0, 28, 1360, 230);
            HorizontalLayoutGroup bottomLayout = bottomRow.gameObject.AddComponent<HorizontalLayoutGroup>();
            bottomLayout.spacing = 18;
            bottomLayout.childControlHeight = true;
            bottomLayout.childControlWidth = true;
            bottomLayout.childForceExpandHeight = true;
            bottomLayout.childForceExpandWidth = false;

            TurnPhaseView turnPhase = TurnPhaseView.Create(bottomRow);
            AddCommandButton(bottomRow, "Desfazer", UIBuilder.Warning, UndoLast);
            AddCommandButton(bottomRow, "Log", UIBuilder.Accent, () => { });
            AddCommandButton(bottomRow, "Menu", new Color(0.25f, 0.28f, 0.32f, 1f), SceneLoader.LoadMainMenu);

            LogPanelView logPanel = LogPanelView.Create(canvas);
            ToastView toast = ToastView.Create(canvas);
            Button logButton = bottomRow.Find("Log").GetComponent<Button>();
            logButton.onClick.RemoveAllListeners();

            HUDView view = canvas.gameObject.AddComponent<HUDView>();
            view._playerA = playerA;
            view._playerB = playerB;
            view._turnPhase = turnPhase;
            view._logPanel = logPanel;
            view._toast = toast;
            logButton.onClick.AddListener(view._logPanel.Toggle);
            AppServices.MatchStateManager.StateChanged += view.Refresh;
            view.Refresh(AppServices.MatchStateManager.State);
            return view;
        }

        private static void AddCommandButton(Transform parent, string label, Color color, UnityEngine.Events.UnityAction action)
        {
            Button button = UIBuilder.CreateButton(parent, label, label, color);
            button.gameObject.AddComponent<LayoutElement>().preferredWidth = 230;
            button.onClick.AddListener(action);
        }

        private static void UndoLast()
        {
            if (AppServices.UndoService.TryUndo(out string description))
            {
                AppServices.LogService.Add(AppServices.MatchStateManager.State, "Acao desfeita", description);
                AppServices.MatchStateManager.NotifyStateChanged();
            }
        }

        private void Refresh(MagicARAssistant.Data.MatchState state)
        {
            _playerA.Refresh();
            _playerB.Refresh();
            _turnPhase.Refresh();
            _logPanel.Refresh(state);
        }

        public void ShowToast(string message)
        {
            _toast.Show(message);
        }
    }
}
