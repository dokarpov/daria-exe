using UnityEngine;
using TMPro;

namespace DariaExe
{
    public sealed class ResearchLabGame : MonoBehaviour
    {
        public enum ResearchStage { Observe, Talk, Verify, Complete }

        public static ResearchLabGame Instance { get; private set; }

        [SerializeField] Transform player;
        [SerializeField] TMP_Text objectiveText;
        [SerializeField] TMP_Text promptText;
        [SerializeField] TMP_Text feedbackText;
        [SerializeField] TMP_Text progressText;
        [SerializeField] float interactionRadius = 1.35f;

        Interactable current;
        ResearchStage stage = ResearchStage.Observe;

        public ResearchStage Stage => stage;

        void Awake()
        {
            Instance = this;
            RefreshUI("Move through the lab. Interactions appear only when you are close enough.");
        }

        void Update()
        {
            current = FindNearestAvailableInteractable();
            if (promptText == null) return;

            bool show = current != null;
            promptText.gameObject.SetActive(show);
            if (show)
                promptText.text = "[ACT]  " + current.Prompt;
        }

        public bool CanInteract(string id)
        {
            return stage switch
            {
                ResearchStage.Observe => id == "observe",
                ResearchStage.Talk => id == "researcher",
                ResearchStage.Verify => id == "verify",
                _ => false
            };
        }

        Interactable FindNearestAvailableInteractable()
        {
            if (player == null) return null;

            var all = FindObjectsOfType<Interactable>();
            Interactable nearest = null;
            float best = interactionRadius;

            foreach (var item in all)
            {
                if (!CanInteract(item.Id)) continue;
                float d = Vector2.Distance(player.position, item.transform.position);
                if (d <= best)
                {
                    best = d;
                    nearest = item;
                }
            }
            return nearest;
        }

        public void TryInteract()
        {
            if (current != null) current.Activate();
        }

        public void ResolveInteraction(Interactable source, string message)
        {
            if (source == null || !CanInteract(source.Id)) return;

            if (source.Id == "observe") stage = ResearchStage.Talk;
            else if (source.Id == "researcher") stage = ResearchStage.Verify;
            else if (source.Id == "verify") stage = ResearchStage.Complete;

            source.gameObject.SetActive(false);
            RefreshUI(message);
        }

        void RefreshUI(string feedback)
        {
            if (feedbackText != null)
                feedbackText.text = feedback;

            if (progressText != null)
                progressText.text = stage == ResearchStage.Complete
                    ? "RESEARCH COMPLETE  ·  3 / 3 evidence steps"
                    : $"RESEARCH  ·  {(int)stage} / 3 evidence steps";

            if (objectiveText == null) return;

            objectiveText.text = stage switch
            {
                ResearchStage.Observe => "1. OBSERVE  ·  Go to the observation station",
                ResearchStage.Talk => "2. FOLLOW UP  ·  Talk to the researcher",
                ResearchStage.Verify => "3. VERIFY  ·  Check the evidence console",
                ResearchStage.Complete => "RESEARCH COMPLETE  ·  Continue to Analytics",
                _ => "Research Lab"
            };
        }
    }
}
