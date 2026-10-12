#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class ResearchLabProjectSetup
{
    const string ScenePath = "Assets/Scenes/ResearchLab.unity";

    static ResearchLabProjectSetup()
    {
        EditorApplication.delayCall += AutoEnsureScene;
    }

    static void AutoEnsureScene()
    {
        if (System.IO.File.Exists(ScenePath))
        {
            if (SceneManager.GetActiveScene().path != ScenePath)
                EditorSceneManager.OpenScene(ScenePath);
            return;
        }
        RebuildScene();
    }

    [MenuItem("DARIA.EXE/Rebuild Research Lab Scene")]
    public static void RebuildScene()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var camGo = new GameObject("Main Camera");
        var cam = camGo.AddComponent<Camera>();
        camGo.tag = "MainCamera";
        cam.orthographic = true;
        cam.orthographicSize = 5.3f;
        cam.transform.position = new Vector3(0, 0, -10);

        var root = new GameObject("Research Lab Vertical Slice");
        root.AddComponent<ResearchLabController>();

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

        if (SceneManager.GetActiveScene().path != ScenePath)
            EditorSceneManager.OpenScene(ScenePath);
    }
}
#endif
