using System.Collections.Generic;
using UnityEngine;

public class ResearchLabController : MonoBehaviour
{
    [System.Serializable]
    class Signal
    {
        public GameObject go;
        public float ttl;
        public int id;
    }

    Transform player;
    Rigidbody2D body;
    readonly List<Transform> npcs = new();
    readonly List<Signal> signals = new();
    readonly HashSet<int> collected = new();

    Sprite playerSprite;
    Sprite scientistSprite;
    Sprite signalSprite;

    float spawnTimer = 1.5f;
    bool complete;

    const float Speed = 4.25f;
    readonly Vector2[] npcAnchors = {
        new(-3.8f, 1.7f),
        new( 0.4f, 0.7f),
        new( 3.7f, 1.45f)
    };

    void Awake()
    {
        BuildWorld();
    }

    void BuildWorld()
    {
        Camera.main.backgroundColor = new Color(0.015f, 0.02f, 0.045f);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5.3f;

        AddFullScreenSprite("Research Lab", "Research/research_lab", 0, Vector3.zero, 1f);
        AddFullScreenSprite("Light Pass", "Research/light_pass", 18, Vector3.zero, .28f);
        AddFullScreenSprite("Foreground", "Research/foreground", 30, Vector3.zero, 1f);

        var playerSheet = Resources.Load<Texture2D>("Research/player_sheet");
        var scientistSheet = Resources.Load<Texture2D>("Research/scientist_sheet");
        playerSprite = FirstFrame(playerSheet, 10, 4);
        scientistSprite = FirstFrame(scientistSheet, 10, 3);
        signalSprite = MakeDiscSprite();

        player = MakeActor("Player", playerSprite, new Vector2(-1.0f, -2.65f), 12, 0.9f);
        body = player.gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.freezeRotation = true;
        var pc = player.gameObject.AddComponent<CapsuleCollider2D>();
        pc.size = new Vector2(.48f, .72f);

        for (int i = 0; i < npcAnchors.Length; i++)
        {
            var npc = MakeActor("Researcher " + (i + 1), scientistSprite, npcAnchors[i], 9, .78f);
            npcs.Add(npc);
        }

        CreateBoundaries();
        CreateHotspots();
    }

    void Update()
    {
        Vector2 input = new(
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? -1 :
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1 : 0,
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? -1 :
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1 : 0
        );
        if (input.sqrMagnitude > 1) input.Normalize();
        body.linearVelocity = input * Speed;

        float t = Time.time;
        for (int i = 0; i < npcs.Count; i++)
        {
            var a = npcAnchors[i];
            npcs[i].position = new Vector3(
                a.x + Mathf.Sin(t * (0.42f + i * .08f) + i) * .65f,
                a.y + Mathf.Cos(t * (0.53f + i * .07f) + i) * .28f,
                0
            );
        }

        if (!complete)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                int id = Random.Range(0, 3);
                SpawnSignal(id, npcs[id].position + new Vector3(.5f, -.1f, 0));
                spawnTimer = Random.Range(1.6f, 2.8f);
            }
        }

        for (int i = signals.Count - 1; i >= 0; i--)
        {
            var s = signals[i];
            s.ttl -= Time.deltaTime;
            float pulse = 1f + Mathf.Sin(Time.time * 8f) * .12f;
            if (s.go) s.go.transform.localScale = Vector3.one * (.34f * pulse);

            if (s.go && Vector2.Distance(player.position, s.go.transform.position) < .55f)
            {
                collected.Add(s.id);
                Destroy(s.go);
                signals.RemoveAt(i);
                if (collected.Count == 3) complete = true;
                continue;
            }

            if (s.ttl <= 0)
            {
                if (s.go) Destroy(s.go);
                signals.RemoveAt(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            collected.Clear();
            complete = false;
        }
    }

    void SpawnSignal(int id, Vector3 p)
    {
        if (collected.Contains(id)) return;
        foreach (var s in signals) if (s.id == id) return;

        var go = new GameObject("Behaviour signal " + id);
        go.transform.position = p;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = signalSprite;
        sr.sortingOrder = 16;
        sr.color = id == 0 ? new Color(.45f, .95f, 1f) :
                   id == 1 ? new Color(1f, .36f, .72f) :
                             new Color(1f, .72f, .27f);
        signals.Add(new Signal { go = go, ttl = 4.0f, id = id });
    }

    void CreateHotspots()
    {
        var props = Resources.Load<Texture2D>("Research/props");
        if (!props) return;
        var sprite = FirstFrame(props, 4, 3);
        var console = MakeActor("Observation Console", sprite, new Vector2(.35f, -.35f), 6, .7f);
        console.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1.3f, .7f);
    }

    void CreateBoundaries()
    {
        AddWall("Left", new Vector2(-5.65f, 0), new Vector2(.25f, 8.5f));
        AddWall("Right", new Vector2(5.65f, 0), new Vector2(.25f, 8.5f));
        AddWall("Top", new Vector2(0, 4.65f), new Vector2(11.5f, .25f));
        AddWall("Bottom", new Vector2(0, -4.45f), new Vector2(11.5f, .25f));

        AddWall("Central workstation", new Vector2(.4f, .15f), new Vector2(3.2f, 1.0f));
        AddWall("Glass lab", new Vector2(3.75f, 2.45f), new Vector2(2.8f, 1.65f));
        AddWall("Left benches", new Vector2(-4.0f, .05f), new Vector2(1.2f, 2.4f));
    }

    void AddWall(string n, Vector2 pos, Vector2 size)
    {
        var go = new GameObject(n);
        go.transform.position = pos;
        var c = go.AddComponent<BoxCollider2D>();
        c.size = size;
    }

    Transform MakeActor(string name, Sprite sprite, Vector2 pos, int order, float scale)
    {
        var go = new GameObject(name);
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * scale;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = order;
        return go.transform;
    }

    void AddFullScreenSprite(string name, string resource, int order, Vector3 pos, float alpha)
    {
        var sprite = Resources.Load<Sprite>(resource);
        if (!sprite) return;
        var go = new GameObject(name);
        go.transform.position = pos;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = order;
        sr.color = new Color(1,1,1,alpha);

        var bounds = sr.sprite.bounds.size;
        float targetW = 11.6f;
        float targetH = 9.2f;
        float scale = Mathf.Max(targetW / bounds.x, targetH / bounds.y);
        go.transform.localScale = Vector3.one * scale;
    }

    Sprite FirstFrame(Texture2D tex, int cols, int rows)
    {
        if (!tex) return null;
        int w = tex.width / cols;
        int h = tex.height / rows;
        var rect = new Rect(0, tex.height - h, w, h);
        return Sprite.Create(tex, rect, new Vector2(.5f,.18f), 64f, 0, SpriteMeshType.FullRect);
    }

    Sprite MakeDiscSprite()
    {
        var tex = new Texture2D(16, 16, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        for (int y=0;y<16;y++) for (int x=0;x<16;x++)
        {
            float d = Vector2.Distance(new Vector2(x,y), new Vector2(7.5f,7.5f));
            tex.SetPixel(x,y, d < 6.5f ? Color.white : Color.clear);
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0,0,16,16), Vector2.one*.5f, 16);
    }

    void OnGUI()
    {
        var box = new GUIStyle(GUI.skin.box) {
            alignment = TextAnchor.UpperLeft,
            fontSize = Mathf.RoundToInt(Screen.height * .024f),
            padding = new RectOffset(16,16,12,12),
            normal = { textColor = Color.white }
        };
        GUI.Box(new Rect(18,18,Mathf.Min(520,Screen.width-36),105),
            complete
            ? "RESEARCH LAB — COMPLETE\n3 / 3 behaviour signals collected\nThe evidence loop is ready for Analytics."
            : $"RESEARCH LAB\nCatch 3 behaviour signals  ·  {collected.Count}/3\nWASD / arrows to move · signals disappear after 4 sec",
            box);

        if (complete)
        {
            var doneStyle = new GUIStyle(GUI.skin.label) {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.RoundToInt(Screen.height * .03f),
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(.55f,1f,.7f) }
            };
            GUI.Label(new Rect(Screen.width/2f-260, Screen.height-90, 520, 60),
                "EVIDENCE HUNTER · Research complete", doneStyle);
        }
    }
}
