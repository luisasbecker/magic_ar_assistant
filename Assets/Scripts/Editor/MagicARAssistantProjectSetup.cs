using System.IO;
using System.Linq;
using MagicARAssistant.App;
using MagicARAssistant.AR;
using MagicARAssistant.UI;
using UnityEditor;
using UnityEngine;

namespace MagicARAssistant.Editor
{
    public static class MagicARAssistantProjectSetup
    {
        private static readonly string[] ScenePaths =
        {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/ARMatchScene.unity",
            "Assets/Scenes/ManualModeScene.unity",
            "Assets/Scenes/SettingsScene.unity",
            "Assets/Scenes/MatchSummaryScene.unity"
        };

        [MenuItem("Magic AR Assistant/Preparar projeto para build")]
        public static void PrepareProjectForBuild()
        {
            ConfigureBuildScenes();
            ConfigureReferenceTextures();
            ConfigurePlayerSettings();
            CreateOverlayPrefabPlaceholders();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Magic AR Assistant: projeto preparado. Verifique XR Plug-in Management para ARCore/ARKit antes do build.");
        }

        [MenuItem("Magic AR Assistant/Abrir cena inicial")]
        public static void OpenMainMenuScene()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MainMenuScene.unity");
        }

        private static void ConfigureBuildScenes()
        {
            EditorBuildSettings.scenes = ScenePaths
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();
        }

        private static void ConfigureReferenceTextures()
        {
            foreach (string texturePath in Directory.GetFiles("Assets/Resources/ReferenceImages", "*.png"))
            {
                TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.textureType = TextureImporterType.Default;
                importer.isReadable = true;
                importer.mipmapEnabled = false;
                importer.maxTextureSize = 512;
                importer.SaveAndReimport();
            }
        }

        private static void ConfigurePlayerSettings()
        {
            PlayerSettings.productName = "Magic AR Assistant";
            PlayerSettings.companyName = "Luisa Becker Santos";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "br.edu.prototype.magicarassistant");
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "br.edu.prototype.magicarassistant");
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.iOS.cameraUsageDescription = PermissionService.CameraPrivacyMessage;
            PlayerSettings.iOS.requiresARKit = true;
        }

        private static void CreateOverlayPrefabPlaceholders()
        {
            Directory.CreateDirectory("Assets/Prefabs/AR");

            CreatePrefabIfMissing<CardOverlayView>("Assets/Prefabs/AR/CardOverlayView.prefab", "CardOverlayView");
            CreatePrefabIfMissing<MarkerOverlayView>("Assets/Prefabs/AR/MarkerOverlayView.prefab", "MarkerOverlayView");
            CreatePrefabIfMissing<AROverlayFactory>("Assets/Prefabs/AR/AROverlayFactory.prefab", "AROverlayFactory");
        }

        private static void CreatePrefabIfMissing<T>(string path, string name) where T : Component
        {
            if (File.Exists(path))
            {
                return;
            }

            GameObject prefabRoot = new(name);
            prefabRoot.AddComponent<T>();
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            Object.DestroyImmediate(prefabRoot);
        }
    }
}
