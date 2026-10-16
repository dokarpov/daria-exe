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
            LoadInto(background, backgroundPath, 100f);
            LoadInto(foreground, foregroundPath, 100f);
            LoadInto(playerRenderer, playerPath, 100f);
            LoadInto(scientistRenderer, scientistPath, 100f);
        }

        static void LoadInto(SpriteRenderer renderer, string resourcePath, float pixelsPerUnit)
        {
            if (renderer == null) return;
            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                Debug.LogError("Missing DARIA.EXE art asset at Resources/" + resourcePath, renderer);
                return;
            }

            texture.filterMode = FilterMode.Point;
            var sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(.5f, .5f),
                pixelsPerUnit,
                0,
                SpriteMeshType.FullRect
            );
            renderer.sprite = sprite;
        }
    }
}
