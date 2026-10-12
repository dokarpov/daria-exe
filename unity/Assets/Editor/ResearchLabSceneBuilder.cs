using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DariaExe;

namespace DariaExe.Editor
{
    public static class ResearchLabSceneBuilder
    {
        const string ScenePath = "Assets/Scenes/ResearchLab.unity";

        [InitializeOnLoadMethod]
        static void AutoCreate()
        {
            EditorApplication.delayCall += () =>
            {
                if (!File.Exists(ScenePath))
                    Build();
            };
        }

        [MenuItem("DARIA.EXE/Build Research Lab")]
        public static void Build()
        {
            Directory.CreateDirectory("Assets/Scenes");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraGo = new GameObject("Main Camera");
            var camera = cameraGo.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5.4f;
            camera.backgroundColor = new Color(0.025f, 0.035f, 0.055f);
            cameraGo.tag = "MainCamera";
            cameraGo.transform.position = new Vector3(0, 0, -10);

            CreateArtSlot("ART_Background", -20);
            CreateArtSlot("ART_Environment", -10);
            CreateArtSlot("ART_Foreground", 20);
            CreateArtSlot("ART_EmissiveFX", 30);

            var player = new GameObject("Player");
            player.transform.position = new Vector3(-5.2f, -2.1f, 0);
            player.AddComponent<Rigidbody2D>();
            var playerCollider = player.AddComponent<CapsuleCollider2D>();
            playerCollider.size = new Vector2(.55f, .9f);
            player.AddComponent<PlayerController>();

            var station = new GameObject("Observation Station");
            station.transform.position = new Vector3(-1.6f, .6f, 0);
            var stationCollider = station.AddComponent<BoxCollider2D>();
            stationCollider.size = new Vector2(1.3f, 1.1f);
            stationCollider.isTrigger = true;
            station.AddComponent<Interactable>();

            CreateWall("Wall Top", new Vector2(0, 4.7f), new Vector2(15, .4f));
            CreateWall("Wall Bottom", new Vector2(0, -4.7f), new Vector2(15, .4f));
            CreateWall("Wall Left", new Vector2(-7.3f, 0), new Vector2(.4f, 10));
            CreateWall("Wall Right", new Vector2(7.3f, 0), new Vector2(.4f, 10));

            var canvasGo = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1440, 900);
            scaler.matchWidthOrHeight = .5f;

            var objective = CreateText(canvasGo.transform, "Objective", new Vector2(36, -34), new Vector2(760, 60), 30, TextAlignmentOptions.TopLeft);
            var feedback = CreateText(canvasGo.transform, "Controls / Feedback", new Vector2(36, 44), new Vector2(900, 50), 22, TextAlignmentOptions.BottomLeft, false);
            var prompt = CreateText(canvasGo.transform, "Interact Prompt", new Vector2(0, 110), new Vector2(500, 60), 28, TextAlignmentOptions.Center, false);
            prompt.rectTransform.anchorMin = prompt.rectTransform.anchorMax = new Vector2(.5f, 0);
            prompt.rectTransform.anchoredPosition = new Vector2(0, 110);

            var game = new GameObject("ResearchLabGame").AddComponent<ResearchLabGame>();
            SetSerialized(game, "player", player.transform);
            SetSerialized(game, "objectiveText", objective);
            SetSerialized(game, "promptText", prompt);
            SetSerialized(game, "feedbackText", feedback);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log("DARIA.EXE Research Lab scene built at " + ScenePath);
        }

        static void CreateArtSlot(string name, int sortingOrder)
        {
            var go = new GameObject(name);
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = sortingOrder;
        }

        static void CreateWall(string name, Vector2 pos, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            var col = go.AddComponent<BoxCollider2D>();
            col.size = size;
        }

        static TMP_Text CreateText(Transform parent, string name, Vector2 pos, Vector2 size, float fontSize, TextAlignmentOptions alignment, bool top = true)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<TextMeshProUGUI>();
            t.fontSize = fontSize;
            t.color = Color.white;
            t.alignment = alignment;
            var rt = t.rectTransform;
            rt.anchorMin = top ? new Vector2(0, 1) : new Vector2(0, 0);
            rt.anchorMax = rt.anchorMin;
            rt.pivot = top ? new Vector2(0, 1) : new Vector2(0, 0);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
            return t;
        }

        static void SetSerialized(Object target, string property, Object value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(property).objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
