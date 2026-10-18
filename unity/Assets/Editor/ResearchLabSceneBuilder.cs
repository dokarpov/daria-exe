using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

            var background = CreateArtSlot("ART_Background", -20);
            CreateArtSlot("ART_Environment", -10);
            var foreground = CreateArtSlot("ART_Foreground", 20);
            CreateArtSlot("ART_EmissiveFX", 30);

            var player = new GameObject("Player");
            player.transform.position = new Vector3(-5.2f, -2.1f, 0);
            player.AddComponent<Rigidbody2D>();
            var playerCollider = player.AddComponent<CapsuleCollider2D>();
            playerCollider.size = new Vector2(.55f, .9f);
            player.AddComponent<PlayerController>();
            var playerRenderer = player.AddComponent<SpriteRenderer>();
            playerRenderer.sortingOrder = 5;

            var scientist = new GameObject("Researcher");
            scientist.transform.position = new Vector3(2.7f, 1.1f, 0);
            var scientistRenderer = scientist.AddComponent<SpriteRenderer>();
            scientistRenderer.sortingOrder = 4;

            var station = CreateInteractable(
                "Observation Station",
                new Vector3(-2.8f, 1.2f, 0),
                "observe",
                "Observe user behaviour",
                "You saw where the user hesitates. Follow up with the researcher."
            );

            var researcherInteraction = CreateInteractable(
                "Researcher Interaction",
                new Vector3(2.7f, 1.1f, 0),
                "researcher",
                "Ask about the hesitation",
                "The researcher confirms the pattern is repeated, not a one-off. Verify it in the evidence console."
            );

            var verify = CreateInteractable(
                "Evidence Console",
                new Vector3(3.8f, -2.0f, 0),
                "verify",
                "Verify the repeated signal",
                "Verified: the behaviour repeats across sessions. Research is complete."
            );

            CreateWall("Wall Top", new Vector2(0, 4.7f), new Vector2(15, .4f));
            CreateWall("Wall Bottom", new Vector2(0, -4.7f), new Vector2(15, .4f));
            CreateWall("Wall Left", new Vector2(-7.3f, 0), new Vector2(.4f, 10));
            CreateWall("Wall Right", new Vector2(7.3f, 0), new Vector2(.4f, 10));

            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

            var canvasGo = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1440, 900);
            scaler.matchWidthOrHeight = .5f;

            var objective = CreateText(canvasGo.transform, "Objective", new Vector2(36, -34), new Vector2(880, 60), 30, TextAlignmentOptions.TopLeft);
            var progress = CreateText(canvasGo.transform, "Progress", new Vector2(36, -84), new Vector2(640, 40), 18, TextAlignmentOptions.TopLeft);
            var feedback = CreateText(canvasGo.transform, "Feedback", new Vector2(36, 44), new Vector2(980, 50), 22, TextAlignmentOptions.BottomLeft, false);
            var prompt = CreateText(canvasGo.transform, "Interact Prompt", new Vector2(0, 110), new Vector2(620, 60), 28, TextAlignmentOptions.Center, false);
            prompt.rectTransform.anchorMin = prompt.rectTransform.anchorMax = new Vector2(.5f, 0);
            prompt.rectTransform.anchoredPosition = new Vector2(0, 110);

            var art = new GameObject("ResearchLabArt").AddComponent<ResearchLabArtLoader>();
            SetSerialized(art, "background", background);
            SetSerialized(art, "foreground", foreground);
            SetSerialized(art, "playerRenderer", playerRenderer);
            SetSerialized(art, "scientistRenderer", scientistRenderer);

            var game = new GameObject("ResearchLabGame").AddComponent<ResearchLabGame>();
            SetSerialized(game, "player", player.transform);
            SetSerialized(game, "objectiveText", objective);
            SetSerialized(game, "promptText", prompt);
            SetSerialized(game, "feedbackText", feedback);
            SetSerialized(game, "progressText", progress);

            CreateMobileControls(canvasGo.transform, player.GetComponent<PlayerController>());

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log("DARIA.EXE Research Lab scene built at " + ScenePath);
        }

        static Interactable CreateInteractable(string name, Vector3 position, string id, string prompt, string feedback)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            var collider = go.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(1.2f, 1.0f);
            collider.isTrigger = true;

            var item = go.AddComponent<Interactable>();
            SetSerializedString(item, "id", id);
            SetSerializedString(item, "prompt", prompt);
            SetSerializedString(item, "feedback", feedback);
            return item;
        }

        static void CreateMobileControls(Transform parent, PlayerController player)
        {
            var controls = new GameObject("Mobile Controls", typeof(RectTransform));
            controls.transform.SetParent(parent, false);

            CreateControlButton(controls.transform, "Left", new Vector2(76, 92), "◀", MobileControlButton.Action.Left, player);
            CreateControlButton(controls.transform, "Right", new Vector2(180, 92), "▶", MobileControlButton.Action.Right, player);
            CreateControlButton(controls.transform, "Up", new Vector2(128, 144), "▲", MobileControlButton.Action.Up, player);
            CreateControlButton(controls.transform, "Down", new Vector2(128, 40), "▼", MobileControlButton.Action.Down, player);
            CreateControlButton(controls.transform, "ACT", new Vector2(-92, 92), "ACT", MobileControlButton.Action.Interact, player, true);
        }

        static void CreateControlButton(Transform parent, string name, Vector2 position, string label, MobileControlButton.Action action, PlayerController player, bool right = false)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button), typeof(MobileControlButton));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = right ? new Vector2(1, 0) : new Vector2(0, 0);
            rt.pivot = new Vector2(.5f, .5f);
            rt.anchoredPosition = position;
            rt.sizeDelta = new Vector2(82, 82);

            var image = go.GetComponent<Image>();
            image.color = new Color(.04f, .08f, .11f, .72f);

            var control = go.GetComponent<MobileControlButton>();
            control.Configure(player, action);

            var text = CreateText(go.transform, "Label", Vector2.zero, new Vector2(82, 82), 22, TextAlignmentOptions.Center, false);
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.pivot = new Vector2(.5f, .5f);
            text.rectTransform.anchoredPosition = Vector2.zero;
            text.rectTransform.sizeDelta = Vector2.zero;
            text.text = label;
            text.raycastTarget = false;
        }

        static void SetSerializedString(Object target, string property, string value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(property).stringValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static SpriteRenderer CreateArtSlot(string name, int sortingOrder)
        {
            var go = new GameObject(name);
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = sortingOrder;
            return renderer;
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
