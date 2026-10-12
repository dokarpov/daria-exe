using UnityEngine;
using UnityEngine.InputSystem;

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
            var keyboard = Keyboard.current;
            if (keyboard == null) { move = Vector2.zero; return; }

            move = Vector2.zero;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move.x -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move.x += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) move.y -= 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) move.y += 1f;
            move = Vector2.ClampMagnitude(move, 1f);

            if ((keyboard.eKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame)
                && ResearchLabGame.Instance != null)
                ResearchLabGame.Instance.TryInteract();
        }

        void FixedUpdate() => body.velocity = move * speed;

        void OnDisable()
        {
            move = Vector2.zero;
            if (body != null) body.velocity = Vector2.zero;
        }
    }
}
