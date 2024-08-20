using UnityEngine;

namespace Core.Cutscene
{
    [CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene/New Cutscene")]
    public class Cutscene : ScriptableObject
    {
        public CutsceneSlide[] slides;
    }
}