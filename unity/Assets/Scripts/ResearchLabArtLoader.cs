using System.Collections.Generic;
using UnityEngine;

namespace DariaExe
{
    public sealed class ResearchLabArtLoader : MonoBehaviour
    {
        [Header("Prepared raster assets")]
        [SerializeField] SpriteRenderer background;
        [SerializeField] SpriteRenderer foreground;
        [SerializeField] SpriteRenderer playerRenderer;
        [SerializeField] SpriteRenderer scientistRenderer;

        [SerializeField] string backgroundPath = "Art/ResearchLab/neon_rain_research_lab";
        [SerializeField] string foregroundPath = "Art/ResearchLab/neon_industrial_atrium_foreground_frame";
        [SerializeField] string playerPath = "Art/Characters/player";
        [SerializeField] string scientistPath = "Art/Characters/scientist";

        void Awake()
        {
            LoadEnvironment(background, backgroundPath);
            LoadEnvironment(foreground, foregroundPath);
            LoadAtlas(playerRenderer, playerPath, 8, 8f);
            LoadAtlas(scientistRenderer, scientistPath, 4, 4.5f);
        }

        static void LoadEnvironment(SpriteRenderer renderer, string resourcePath)
        {
            if (renderer == null) return;
            var texture = LoadTexture(resourcePath, renderer);
            if (texture == null) return;

            renderer.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(.5f, .5f),
                100f,
                0,
                SpriteMeshType.FullRect
            );

            FitToCamera(renderer);
        }

        static void LoadAtlas(SpriteRenderer renderer, string resourcePath, int frameCount, float fps)
        {
            if (renderer == null) return;
            var texture = LoadTexture(resourcePath, renderer);
            if (texture == null) return;

            if (frameCount < 1 || texture.width % frameCount != 0)
            {
                Debug.LogError($"Invalid DARIA.EXE atlas {resourcePath}: width {texture.width} is not divisible by {frameCount}.", renderer);
                return;
            }

            int frameWidth = texture.width / frameCount;
            var frames = new List<Sprite>(frameCount);
            for (int i = 0; i < frameCount; i++)
            {
                frames.Add(Sprite.Create(
                    texture,
                    new Rect(i * frameWidth, 0, frameWidth, texture.height),
                    new Vector2(.5f, .08f),
                    100f,
                    0,
                    SpriteMeshType.FullRect
                ));
            }

            renderer.sprite = frames[0];
            var animator = renderer.gameObject.AddComponent<RasterAtlasAnimator>();
            animator.Configure(frames.ToArray(), fps);
        }

        static Texture2D LoadTexture(string resourcePath, Object context)
        {
            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                Debug.LogError("Missing DARIA.EXE art asset at Resources/" + resourcePath, context);
                return null;
            }

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            return texture;
        }

        static void FitToCamera(SpriteRenderer renderer)
        {
            var camera = Camera.main;
            if (camera == null || renderer.sprite == null) return;

            var bounds = renderer.sprite.bounds.size;
            float cameraHeight = camera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * camera.aspect;
            float scale = Mathf.Max(cameraWidth / bounds.x, cameraHeight / bounds.y);
            renderer.transform.localScale = Vector3.one * scale;
        }
    }

    public sealed class RasterAtlasAnimator : MonoBehaviour
    {
        SpriteRenderer target;
        Sprite[] frames;
        float fps;
        int index;
        float nextFrame;

        public void Configure(Sprite[] sprites, float framesPerSecond)
        {
            target = GetComponent<SpriteRenderer>();
            frames = sprites;
            fps = Mathf.Max(.1f, framesPerSecond);
            index = 0;
            nextFrame = Time.unscaledTime + 1f / fps;
        }

        void Update()
        {
            if (target == null || frames == null || frames.Length < 2) return;
            if (Time.unscaledTime < nextFrame) return;

            index = (index + 1) % frames.Length;
            target.sprite = frames[index];
            nextFrame = Time.unscaledTime + 1f / fps;
        }
    }
}
