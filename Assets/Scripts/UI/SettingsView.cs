using MagicARAssistant.App;
using UnityEngine;
using UnityEngine.UI;

namespace MagicARAssistant.UI
{
    public sealed class SettingsView : MonoBehaviour
    {
        private ToastView _toast;
        private Text _compatibility;

        public void Build()
        {
            Canvas canvas = UIBuilder.CreateScreenCanvas("SettingsCanvas");
            RectTransform background = UIBuilder.CreatePanel(canvas.transform, "Background", UIBuilder.Background);
            UIBuilder.Stretch(background);

            Text title = UIBuilder.CreateText(canvas.transform, "Title", "Configuracoes", 58, FontStyle.Bold);
            UIBuilder.SetTopLeft(title.GetComponent<RectTransform>(), 42, 42, 900, 90);
            Button menu = UIBuilder.CreateButton(canvas.transform, "Menu", "Menu", UIBuilder.Warning);
            UIBuilder.SetTopLeft(menu.GetComponent<RectTransform>(), 1120, 42, 240, 82);
            menu.onClick.AddListener(SceneLoader.LoadMainMenu);

            RectTransform panel = UIBuilder.CreatePanel(canvas.transform, "SettingsPanel", new Color(0.08f, 0.09f, 0.10f, 0.94f));
            UIBuilder.SetTopLeft(panel, 42, 160, 1318, 1820);
            UIBuilder.AddVerticalLayout(panel.gameObject, 18, new RectOffset(24, 24, 24, 24));

            AddToggle(panel, "Modo iniciante", "beginner_mode", true);
            AddDropdownLike(panel, "Tamanho dos overlays", "overlay_size", "medio");
            AddToggle(panel, "Sons", "sound_enabled", true);
            AddToggle(panel, "Vibracao/haptico", "haptics_enabled", true);
            AddToggle(panel, "Persistencia automatica", "autosave_enabled", true);
            AddSlider(panel, "Tempo para ocultar overlay apos perda de tracking", "tracking_hide_delay", 4f, 2f, 10f);

            _compatibility = UIBuilder.CreateText(panel, "Compatibility", "", 28, FontStyle.Normal, TextAnchor.UpperLeft);
            _compatibility.gameObject.AddComponent<LayoutElement>().preferredHeight = 260;
            RefreshCompatibility();

            Text privacy = UIBuilder.CreateText(panel, "Privacy", "Privacidade\nO Magic AR Assistant usa a camera apenas localmente para recursos de Realidade Aumentada. O aplicativo nao salva imagens automaticamente e nao envia dados para servidores. O log da partida fica salvo somente neste dispositivo, salvo exportacao manual.", 28, FontStyle.Normal, TextAnchor.UpperLeft);
            privacy.gameObject.AddComponent<LayoutElement>().preferredHeight = 240;

            AddButton(panel, "Limpar dados locais", UIBuilder.Danger, ClearData);
            AddButton(panel, "Atualizar compatibilidade AR", UIBuilder.Accent, RefreshCompatibility);
            _toast = ToastView.Create(canvas);
        }

        private void AddToggle(Transform parent, string label, string key, bool defaultValue)
        {
            Toggle toggle = new GameObject(label).AddComponent<Toggle>();
            toggle.transform.SetParent(parent, false);
            toggle.gameObject.AddComponent<LayoutElement>().preferredHeight = 86;
            Image background = toggle.gameObject.AddComponent<Image>();
            background.color = new Color(0.14f, 0.16f, 0.18f, 1f);
            Text text = UIBuilder.CreateText(toggle.transform, "Label", label, 32, FontStyle.Bold);
            UIBuilder.Stretch(text.GetComponent<RectTransform>(), 84, 8, 20, 8);
            bool value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
            toggle.isOn = value;
            toggle.onValueChanged.AddListener(enabled => PlayerPrefs.SetInt(key, enabled ? 1 : 0));
        }

        private void AddDropdownLike(Transform parent, string label, string key, string defaultValue)
        {
            string value = PlayerPrefs.GetString(key, defaultValue);
            AddButton(parent, $"{label}: {value}", new Color(0.30f, 0.34f, 0.38f, 1f), () =>
            {
                string current = PlayerPrefs.GetString(key, defaultValue);
                string next = current == "pequeno" ? "medio" : current == "medio" ? "grande" : "pequeno";
                PlayerPrefs.SetString(key, next);
                _toast.Show($"{label}: {next}");
            });
        }

        private void AddSlider(Transform parent, string label, string key, float defaultValue, float min, float max)
        {
            Text text = UIBuilder.CreateText(parent, label, $"{label}: {PlayerPrefs.GetFloat(key, defaultValue):0.0}s", 30, FontStyle.Bold);
            text.gameObject.AddComponent<LayoutElement>().preferredHeight = 60;
            Slider slider = new GameObject(key).AddComponent<Slider>();
            slider.transform.SetParent(parent, false);
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = PlayerPrefs.GetFloat(key, defaultValue);
            slider.gameObject.AddComponent<LayoutElement>().preferredHeight = 70;
            slider.onValueChanged.AddListener(value =>
            {
                PlayerPrefs.SetFloat(key, value);
                text.text = $"{label}: {value:0.0}s";
            });
        }

        private void AddButton(Transform parent, string label, Color color, UnityEngine.Events.UnityAction action)
        {
            Button button = UIBuilder.CreateButton(parent, label, label, color);
            button.gameObject.AddComponent<LayoutElement>().preferredHeight = 92;
            button.onClick.AddListener(action);
        }

        private void ClearData()
        {
            if (AppServices.Storage.ResetLastMatch(out string error))
            {
                PlayerPrefs.DeleteAll();
                _toast.Show("Dados locais limpos.");
            }
            else
            {
                _toast.Show(error);
            }
        }

        private void RefreshCompatibility()
        {
            DeviceCompatibilityService service = gameObject.GetComponent<DeviceCompatibilityService>();
            if (service == null)
            {
                service = gameObject.AddComponent<DeviceCompatibilityService>();
            }

            if (_compatibility != null)
            {
                _compatibility.text = "Compatibilidade AR\n" + service.BuildCompatibilitySummary();
            }
        }
    }
}

