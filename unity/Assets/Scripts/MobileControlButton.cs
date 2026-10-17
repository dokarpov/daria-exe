using UnityEngine;
using UnityEngine.EventSystems;

namespace DariaExe
{
    public sealed class MobileControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public enum Action { Left, Right, Up, Down, Interact }

        [SerializeField] PlayerController player;
        [SerializeField] Action action;

        public void Configure(PlayerController controller, Action value)
        {
            player = controller;
            action = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (player == null) return;
            if (action == Action.Interact)
            {
                ResearchLabGame.Instance?.TryInteract();
                return;
            }
            player.SetTouchDirection(Direction(action), true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (player == null || action == Action.Interact) return;
            player.SetTouchDirection(Direction(action), false);
        }

        public void OnPointerExit(PointerEventData eventData) => OnPointerUp(eventData);

        static Vector2 Direction(Action value) => value switch
        {
            Action.Left => Vector2.left,
            Action.Right => Vector2.right,
            Action.Up => Vector2.up,
            Action.Down => Vector2.down,
            _ => Vector2.zero
        };
    }
}
