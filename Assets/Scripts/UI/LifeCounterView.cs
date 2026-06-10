using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class LifeCounterView : MonoBehaviour
    {
        private string _player;
        private Text _lifeText;

        public static LifeCounterView Create(Transform parent, string player)
        {
            RectTransform panel = UIBuilder.CreatePanel(parent, $"{player}LifePanel", new Color(0.10f, 0.12f, 0.14f, 0.88f));
            panel.sizeDelta = new Vector2(620, 280);
            UIBuilder.AddVerticalLayout(panel.gameObject, 12, new RectOffset(18, 18, 18, 18));
            Text title = UIBuilder.CreateText(panel, "Title", player, 30, FontStyle.Bold, TextAnchor.MiddleCenter);
            title.gameObject.AddComponent<LayoutElement>().preferredHeight = 42;

            Text life = UIBuilder.CreateText(panel, "Life", "20", 58, FontStyle.Bold, TextAnchor.MiddleCenter);
            life.gameObject.AddComponent<LayoutElement>().preferredHeight = 74;

            GameObject buttonsRow = new("ButtonsRow");
            buttonsRow.transform.SetParent(panel, false);
            HorizontalLayoutGroup row = buttonsRow.AddComponent<HorizontalLayoutGroup>();
            row.spacing = 10;
            row.childControlWidth = true;
            row.childForceExpandWidth = true;
            row.childControlHeight = true;
            row.childForceExpandHeight = true;
            buttonsRow.AddComponent<LayoutElement>().preferredHeight = 88;

            AddButton(buttonsRow.transform, "+1", UIBuilder.Accent, () => AppServices.LifeCounterController.ChangeLife(player, 1));
            AddButton(buttonsRow.transform, "-1", UIBuilder.Danger, () => AppServices.LifeCounterController.ChangeLife(player, -1));
            AddButton(buttonsRow.transform, "+5", UIBuilder.Accent, () => AppServices.LifeCounterController.ChangeLife(player, 5));
            AddButton(buttonsRow.transform, "-5", UIBuilder.Danger, () => AppServices.LifeCounterController.ChangeLife(player, -5));
            AddButton(buttonsRow.transform, "20", UIBuilder.Warning, () => AppServices.LifeCounterController.ResetLife(player));

            LifeCounterView view = panel.gameObject.AddComponent<LifeCounterView>();
            view._player = player;
            view._lifeText = life;
            return view;
        }

        public void Refresh()
        {
            int life = _player == "Jogador A"
                ? AppServices.MatchStateManager.State.playerALife
                : AppServices.MatchStateManager.State.playerBLife;
            _lifeText.text = life.ToString();
        }

        private static void AddButton(Transform parent, string label, Color color, UnityEngine.Events.UnityAction action)
        {
            Button button = UIBuilder.CreateButton(parent, $"Button{label}", label, color);
            button.onClick.AddListener(action);
        }
    }
}
