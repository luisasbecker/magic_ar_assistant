using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagicARAssistant.App
{
    public static class SceneLoader
    {
        public const string MainMenuScene = "MainMenuScene";
        public const string ARMatchScene = "ARMatchScene";
        public const string ManualModeScene = "ManualModeScene";
        public const string SettingsScene = "SettingsScene";
        public const string MatchSummaryScene = "MatchSummaryScene";

        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }

        public static void LoadARMatch()
        {
            SceneManager.LoadScene(ARMatchScene);
        }

        public static void LoadManualMode()
        {
            SceneManager.LoadScene(ManualModeScene);
        }

        public static void LoadSettings()
        {
            SceneManager.LoadScene(SettingsScene);
        }

        public static void LoadSummary()
        {
            SceneManager.LoadScene(MatchSummaryScene);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

