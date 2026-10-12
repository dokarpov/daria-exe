using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DariaExe.Editor
{
    public static class WebGLBuild
    {
        public static void Build()
        {
            if (!File.Exists("Assets/Scenes/ResearchLab.unity"))
                ResearchLabSceneBuilder.Build();

            var output = EnvironmentValue("BUILD_PATH", "Build/WebGL");
            Directory.CreateDirectory(output);

            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/ResearchLab.unity" },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new System.Exception($"WebGL build failed: {report.summary.result}");
        }

        static string EnvironmentValue(string name, string fallback)
        {
            var value = System.Environment.GetEnvironmentVariable(name);
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }
    }
}
