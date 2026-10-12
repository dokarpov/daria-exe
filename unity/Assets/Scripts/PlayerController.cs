using UnityEngine;

namespace DariaExe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 4.2f;
        Rigidbody2D body;
        Vector2 move;

        public Vector2 MoveInput => move;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
        }

        void Update()
        {
            move = Vector2.zero;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move.x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move.x += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) move.y -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) move.y += 1f;
            move = Vector2.ClampMagnitude(move, 1f);

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                && ResearchLabGame.Instance != null)
                ResearchLabGame.Instance.TryInteract();
        }

        void FixedUpdate()
        {
            if (body != null) body.velocity = move * speed;
        }

        void OnDisable()
        {
            move = Vector2.zero;
            if (body != null) body.velocity = Vector2.zero;
        }
    }
}
