using UnityEngine;

namespace DariaExe
{
    public sealed class Interactable : MonoBehaviour
    {
        [SerializeField] string prompt = "Inspect station";
        [TextArea] [SerializeField] string feedback = "You found a behaviour signal.";
        [SerializeField] string id = "research-station";

        public string Prompt => prompt;
        public string Id => id;

        public void Activate()
        {
            if (ResearchLabGame.Instance != null)
                ResearchLabGame.Instance.ResolveInteraction(this, feedback);
        }
    }
}
