using System.IO;
using System.Linq;
using UnityEditor;

namespace RingCrisis
{
    public static class Builder
    {
        private static readonly string BuildDirectoryName = "Builds";

        [MenuItem("ビルド/PhotonTutorialをビルドして実行", false, 1)]
        private static void BuildAndRunPhotonTutorial()
        {
            BuildAndRun("PhotonTutorial", "PhotonTutorial");
        }

        [MenuItem("ビルド/RingCrisisをビルドして実行", false, 2)]
        private static void BuildAndRunRingCrisis()
        {
            BuildAndRun("RingCrisis", "Game");
        }

        [MenuItem("ビルド/ビルドフォルダを開く", false, 21)]
        private static void OpenBuildFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildDirectoryName);
        }

        private static void BuildAndRun(string appName, string sceneName)
        {
            var outputDirectory = Path.Combine(BuildDirectoryName, appName);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var targetScene = EditorBuildSettings.scenes.First(scene => Path.GetFileNameWithoutExtension(scene.path) == sceneName);
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var locationPath = Path.Combine(outputDirectory, MakeApplicationFileName(appName, buildTarget));
            var buildOptions = BuildOptions.SymlinkLibraries | BuildOptions.AutoRunPlayer;

            var originalName = PlayerSettings.productName;
            PlayerSettings.productName = appName;
            BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] { targetScene }, locationPath, buildTarget, buildOptions);
            PlayerSettings.productName = originalName;
            AssetDatabase.SaveAssets();
        }

        private static string MakeApplicationFileName(string fileName, BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    return $"{fileName}.exe";
                case BuildTarget.StandaloneOSX:
                    return $"{fileName}.app";
            }
            return fileName;
        }
    }
}
