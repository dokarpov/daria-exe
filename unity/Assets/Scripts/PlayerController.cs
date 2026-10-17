using UnityEngine;

namespace DariaExe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 4.2f;
        Rigidbody2D body;
        Vector2 keyboardMove;
        Vector2 touchMove;

        public Vector2 MoveInput => Vector2.ClampMagnitude(keyboardMove + touchMove, 1f);

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
        }

        void Update()
        {
            keyboardMove = Vector2.zero;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) keyboardMove.x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) keyboardMove.x += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) keyboardMove.y -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) keyboardMove.y += 1f;
            keyboardMove = Vector2.ClampMagnitude(keyboardMove, 1f);

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                && ResearchLabGame.Instance != null)
                ResearchLabGame.Instance.TryInteract();
        }

        public void SetTouchDirection(Vector2 direction, bool pressed)
        {
            if (pressed) touchMove += direction;
            else touchMove -= direction;
            touchMove.x = Mathf.Clamp(touchMove.x, -1f, 1f);
            touchMove.y = Mathf.Clamp(touchMove.y, -1f, 1f);
        }

        void FixedUpdate()
        {
            if (body != null) body.velocity = MoveInput * speed;
        }

        void OnDisable()
        {
            keyboardMove = Vector2.zero;
            touchMove = Vector2.zero;
            if (body != null) body.velocity = Vector2.zero;
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                keyboardMove = Vector2.zero;
                touchMove = Vector2.zero;
            }
        }
    }
}
