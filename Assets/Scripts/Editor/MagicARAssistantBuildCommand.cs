using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MagicARAssistant.Editor
{
    public static class MagicARAssistantBuildCommand
    {
        private static readonly string[] ScenePaths =
        {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/ARMatchScene.unity",
            "Assets/Scenes/ManualModeScene.unity",
            "Assets/Scenes/SettingsScene.unity",
            "Assets/Scenes/MatchSummaryScene.unity"
        };

        [MenuItem("Magic AR Assistant/Build/Android APK")]
        public static void BuildAndroidApk()
        {
            MagicARAssistantProjectSetup.PrepareProjectForBuild();
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            EditorUserBuildSettings.buildAppBundle = false;

            string outputPath = GetArgument("-outputPath", "Builds/Android/MagicARAssistant.apk");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? "Builds/Android");
            RunBuild(BuildTarget.Android, outputPath);
        }

        [MenuItem("Magic AR Assistant/Build/iOS Xcode Project")]
        public static void BuildIosXcodeProject()
        {
            MagicARAssistantProjectSetup.PrepareProjectForBuild();
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.iOS.requiresARKit = true;

            string outputPath = GetArgument("-outputPath", "Builds/iOS/MagicARAssistantXcode");
            Directory.CreateDirectory(outputPath);
            RunBuild(BuildTarget.iOS, outputPath);
        }

        private static void RunBuild(BuildTarget target, string outputPath)
        {
            BuildPlayerOptions options = new()
            {
                scenes = ScenePaths,
                locationPathName = outputPath,
                target = target,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;
            if (summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Build {target} falhou: {summary.result}. Erros: {summary.totalErrors}");
            }

            Debug.Log($"Build {target} concluido em {outputPath}. Tamanho: {summary.totalSize} bytes.");
        }

        private static string GetArgument(string name, string fallback)
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);
            if (index >= 0 && index + 1 < args.Length)
            {
                return args[index + 1];
            }

            return fallback;
        }
    }
}

