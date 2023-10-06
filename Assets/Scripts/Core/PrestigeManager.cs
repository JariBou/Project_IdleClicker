using UnityEngine;

namespace ProjectClicker.Core
{
    public class PrestigeManager : MonoBehaviour
    {

        private int _trophies;
        private int _medals;
        private int _numberOfPrestiges;
        public static PrestigeManager instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }


        public static void PrestigeReset(uint score)
        {
            int value = Mathf.RoundToInt(ScoreConversionFunction(score));
            instance._medals = value;
            instance._trophies = Mathf.RoundToInt(0.4f * value);
        }

        public static float ScoreConversionFunction(uint x)
        {
            return x * 0.01f;
        }

    }
}
