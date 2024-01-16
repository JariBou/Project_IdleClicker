using ProjectClicker.Core;
using ProjectClicker.Saves;
using UnityEngine;

namespace ProjectClicker
{
    [RequireComponent(typeof(LevelsManager))]
    public class PrestigeManager : MonoBehaviour
    {

        private uint _trophies;
        [SerializeField] private uint _medals;
        private int _numberOfPrestiges;
        public static PrestigeManager Instance { get; private set; }

        public uint Medals => _medals;
        public int NumberOfPrestiges => _numberOfPrestiges;
        public uint Trophies => _trophies;

        [SerializeField] private TeamStats _teamStats;
        [SerializeField] private LevelsManager _levelsManager;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            LevelsManager.OnPrestige += OnPrestige;
        }
        
        private void OnDisable()
        {
            LevelsManager.OnPrestige -= OnPrestige;
        }

        private void OnPrestige()
        {
            int currLevel = _levelsManager.CurrentLevel;
            int averageTeamLevel = (int) _teamStats.GetAverageTeamLevel();
            
            PrestigeReset((uint)(currLevel * currLevel + (averageTeamLevel * averageTeamLevel) / 5));
        }


        public static void PrestigeReset(uint score)
        {
            int value = Mathf.RoundToInt(ScoreConversionFunction(score));
            Instance._medals = (uint)value;
            Instance._trophies = (uint)Mathf.RoundToInt(0.4f * value);
        }

        public static float ScoreConversionFunction(uint x)
        {
            return x * 0.01f;
        }

        public void PassData(JsonSaveData saveData)
        {
            _trophies = saveData.Trophies;
            _medals = saveData.Medals;
            _numberOfPrestiges = saveData.NumberOfPrestiges;
        }

        public void RemoveMedals(int upgradeCostInt)
        {
            _medals -= (uint)upgradeCostInt;
        }
    }
}
