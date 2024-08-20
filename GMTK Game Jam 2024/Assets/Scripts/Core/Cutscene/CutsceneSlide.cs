using UnityEngine;

namespace Core.Cutscene
{
    [System.Serializable]
    public class CutsceneSlide
    {
        public Sprite image;
        [TextArea(3, 10)] public string text;
    }
}