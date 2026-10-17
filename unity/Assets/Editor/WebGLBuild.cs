using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DariaExe.Editor
{
    public static class WebGLBuild
    {
        const string ScenePath = "Assets/Scenes/ResearchLab.unity";

        [MenuItem("DARIA.EXE/Build WebGL Release")]
        public static void Build()
        {
            if (!File.Exists(ScenePath))
                ResearchLabSceneBuilder.Build();

            PlayerSettings.companyName = "Daria Karpova";
            PlayerSettings.productName = "DARIA.EXE";
            PlayerSettings.bundleVersion = "0.4.0-rc1";
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.dataCaching = false;

            var output = EnvironmentValue(
                "BUILD_PATH",
                Path.GetFullPath(Path.Combine(Application.dataPath, "../../webgl-release/unity-research-lab"))
            );

            if (Directory.Exists(output))
                Directory.Delete(output, true);
            Directory.CreateDirectory(output);

            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new System.Exception($"WebGL build failed: {report.summary.result}");

            if (!File.Exists(Path.Combine(output, "index.html")))
                throw new System.Exception("WebGL build completed without index.html.");

            File.WriteAllText(
                Path.Combine(output, "release.json"),
                "{\"product\":\"DARIA.EXE\",\"build\":\"unity-research-lab\",\"version\":\"0.4.0-rc1\"}"
            );

            Debug.Log("DARIA.EXE WebGL release built at " + output);
        }

        static string EnvironmentValue(string name, string fallback)
        {
            var value = System.Environment.GetEnvironmentVariable(name);
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }
    }
}
