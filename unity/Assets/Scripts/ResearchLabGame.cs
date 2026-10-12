using UnityEngine;
using TMPro;

namespace DariaExe
{
    public sealed class ResearchLabGame : MonoBehaviour
    {
        public static ResearchLabGame Instance { get; private set; }

        [SerializeField] Transform player;
        [SerializeField] TMP_Text objectiveText;
        [SerializeField] TMP_Text promptText;
        [SerializeField] TMP_Text feedbackText;
        [SerializeField] float interactionRadius = 1.35f;

        Interactable current;
        bool stationDone;

        void Awake()
        {
            Instance = this;
            if (objectiveText != null)
                objectiveText.text = "RESEARCH LAB  ·  Find the observation station";
            if (promptText != null) promptText.gameObject.SetActive(false);
            if (feedbackText != null) feedbackText.text = "Move: WASD / arrows   ·   Interact: E / Space";
        }

        void Update()
        {
            current = FindNearestInteractable();
            if (promptText == null) return;

            promptText.gameObject.SetActive(current != null);
            if (current != null)
                promptText.text = "[E] " + current.Prompt;
        }

        Interactable FindNearestInteractable()
        {
            if (player == null) return null;
            var all = FindObjectsOfType<Interactable>();
            Interactable nearest = null;
            float best = interactionRadius;

            foreach (var item in all)
            {
                float d = Vector2.Distance(player.position, item.transform.position);
                if (d <= best) { best = d; nearest = item; }
            }
            return nearest;
        }

        public void TryInteract()
        {
            if (current != null) current.Activate();
        }

        public void ResolveInteraction(Interactable source, string message)
        {
            if (source == null || stationDone) return;
            stationDone = true;
            if (feedbackText != null) feedbackText.text = message;
            if (objectiveText != null)
                objectiveText.text = "SIGNAL FOUND  ·  Follow the researcher to the next clue";
            source.gameObject.SetActive(false);
        }
    }
}
