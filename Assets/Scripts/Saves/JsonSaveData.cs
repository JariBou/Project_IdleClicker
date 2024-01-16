using System;
using ProjectClicker.Core;
using Object = UnityEngine.Object;

namespace ProjectClicker.Saves
{
    [Serializable]
    public class JsonSaveData
    {
        
        //Levels
        private int _currLevel;
        
        // Gold
        private ulong _gold;
        
        // Prestige
        private uint _trophies;
        private uint _medals;
        private int _numberOfPrestiges;
        
        // TeamStats
        private ChampionLevelDictionary _championLevelDictionary;

        public int CurrLevel => _currLevel;
        public ulong Gold => _gold;
        public uint Trophies => _trophies;
        public uint Medals => _medals;
        public int NumberOfPrestiges => _numberOfPrestiges;

        public ChampionLevelDictionary LevelDictionary => _championLevelDictionary;

        public static JsonSaveData Initialise()
        {
            TeamStats teamStats = Object.FindObjectOfType<TeamStats>();
            JsonSaveData data = new JsonSaveData
            {
                _gold = GoldManager.Instance.Gold,
                
                _trophies = PrestigeManager.Instance.Trophies,
                _medals = PrestigeManager.Instance.Medals,
                _numberOfPrestiges = PrestigeManager.Instance.NumberOfPrestiges,
                
                _currLevel = LevelsManager.Instance.CurrentLevel,
                
                LevelDictionary =
                {
                    items = new ChampionLevelItem[teamStats.Heroes.Count]
                }
            };

            for (int i = 0; i < teamStats.Heroes.Count; i++)
            {
                data.LevelDictionary.items[i] = new ChampionLevelItem
                {
                    championRole = teamStats.Heroes[i].GetRole(),
                    championLevel = teamStats.Heroes[i].HeroLevel
                };
            }
            
            return data;
        }
    }
}