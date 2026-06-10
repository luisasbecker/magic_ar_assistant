using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class MainMenuView : MonoBehaviour
    {
        private ToastView _toast;
        private PermissionService _permissionService;

        public void Build()
        {
            Canvas canvas = UIBuilder.CreateScreenCanvas("MainMenuCanvas");
            RectTransform background = UIBuilder.CreatePanel(canvas.transform, "Background", UIBuilder.Background);
            UIBuilder.Stretch(background);

            RectTransform content = UIBuilder.CreatePanel(canvas.transform, "Content", new Color(0, 0, 0, 0));
            UIBuilder.SetTopLeft(content, 110, 180, 1220, 2140);
            UIBuilder.AddVerticalLayout(content.gameObject, 26, new RectOffset(12, 12, 12, 12));

            Text title = UIBuilder.CreateText(content, "Title", "Magic AR Assistant", 72, FontStyle.Bold, TextAnchor.MiddleCenter);
            title.gameObject.AddComponent<LayoutElement>().preferredHeight = 130;
            Text subtitle = UIBuilder.CreateText(content, "Subtitle", "Assistente em Realidade Aumentada para gerenciamento de vida, marcadores e habilidades em partidas fisicas de TCG.", 34, FontStyle.Normal, TextAnchor.MiddleCenter);
            subtitle.gameObject.AddComponent<LayoutElement>().preferredHeight = 180;

            AddMenuButton(content, "Iniciar partida AR", UIBuilder.Accent, StartAR);
            AddMenuButton(content, "Modo manual", new Color(0.36f, 0.48f, 0.82f, 1f), SceneLoader.LoadManualMode);
            AddMenuButton(content, "Carregar ultima partida", UIBuilder.Warning, LoadLastMatch);
            AddMenuButton(content, "Configuracoes", new Color(0.30f, 0.34f, 0.38f, 1f), SceneLoader.LoadSettings);
            AddMenuButton(content, "Sair", UIBuilder.Danger, SceneLoader.Quit);

            Text privacy = UIBuilder.CreateText(content, "Privacy", PermissionService.CameraPrivacyMessage + "\nLogs ficam apenas neste dispositivo, salvo exportacao manual.", 26, FontStyle.Normal, TextAnchor.MiddleCenter);
            privacy.gameObject.AddComponent<LayoutElement>().preferredHeight = 210;

            _toast = ToastView.Create(canvas);
            _permissionService = gameObject.AddComponent<PermissionService>();
        }

        private void AddMenuButton(Transform parent, string label, Color color, UnityEngine.Events.UnityAction action)
        {
            Button button = UIBuilder.CreateButton(parent, label, label, color);
            button.gameObject.AddComponent<LayoutElement>().preferredHeight = 112;
            button.onClick.AddListener(action);
        }

        private void StartAR()
        {
            StartCoroutine(_permissionService.RequestCameraPermission(granted =>
            {
                if (granted)
                {
                    SceneLoader.LoadARMatch();
                }
                else
                {
                    _toast.Show("Permissao de camera negada. Libere a camera nas configuracoes do sistema para usar o modo AR.");
                }
            }));
        }

        private void LoadLastMatch()
        {
            if (AppServices.Storage.LoadLastMatch(out var state, out string error))
            {
                AppServices.MatchStateManager.SetState(state);
                _toast.Show("Ultima partida carregada.");
            }
            else
            {
                _toast.Show(error);
            }
        }
    }
}

