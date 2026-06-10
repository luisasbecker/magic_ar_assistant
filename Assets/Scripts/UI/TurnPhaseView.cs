using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class TurnPhaseView : MonoBehaviour
    {
        private Text _status;

        public static TurnPhaseView Create(Transform parent)
        {
            RectTransform panel = UIBuilder.CreatePanel(parent, "TurnPhasePanel", new Color(0.10f, 0.12f, 0.14f, 0.88f));
            panel.sizeDelta = new Vector2(840, 210);
            UIBuilder.AddVerticalLayout(panel.gameObject, 14, new RectOffset(18, 18, 18, 18));

            Text status = UIBuilder.CreateText(panel, "Status", "", 32, FontStyle.Bold, TextAnchor.MiddleCenter);
            status.gameObject.AddComponent<LayoutElement>().preferredHeight = 80;
            GameObject row = new("Buttons");
            row.transform.SetParent(panel, false);
            HorizontalLayoutGroup layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 14;
            layout.childControlWidth = true;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            row.AddComponent<LayoutElement>().preferredHeight = 78;

            Button next = UIBuilder.CreateButton(row.transform, "NextPhase", "Proxima fase", UIBuilder.Accent);
            next.onClick.AddListener(() => AppServices.TurnPhaseController.NextPhase());
            Button pass = UIBuilder.CreateButton(row.transform, "PassTurn", "Passar turno", UIBuilder.Warning);
            pass.onClick.AddListener(() => AppServices.TurnPhaseController.PassTurn());

            TurnPhaseView view = panel.gameObject.AddComponent<TurnPhaseView>();
            view._status = status;
            return view;
        }

        public void Refresh()
        {
            var state = AppServices.MatchStateManager.State;
            _status.text = $"Turno {state.turnNumber} | {state.activePlayer} | {state.phase}";
        }
    }
}

