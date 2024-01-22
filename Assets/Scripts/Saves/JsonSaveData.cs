using System;
using System.Globalization;
using ProjectClicker.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectClicker.Saves
{
    [Serializable]
    public class JsonSaveData
    {
        
        //Levels
        [SerializeField] private int _currLevel;
        
        // Gold
        [SerializeField] private ulong _gold;
        
        // Prestige
        [SerializeField] private uint _trophies;
        [SerializeField] private uint _medals;
        [SerializeField] private int _numberOfPrestiges;
        
        // TeamStats
        [SerializeField] private ChampionLevelDictionary _championLevelDictionary = new();
        [SerializeField] private ChampionLevelDictionary _championPrestigeDictionary = new();


        [SerializeField] private string _lastOpened;

        public int CurrLevel => _currLevel;
        public ulong Gold => _gold;
        public uint Trophies => _trophies;
        public uint Medals => _medals;
        public int NumberOfPrestiges => _numberOfPrestiges;

        public ChampionLevelDictionary LevelDictionary => _championLevelDictionary;
        public ChampionLevelDictionary ChampionPrestigeDictionary => _championPrestigeDictionary;
        public string LastOpened => _lastOpened;

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
                
                _championLevelDictionary = new ()
                {
                    items = new ChampionLevelItem[teamStats.Heroes.Count]
                },
                _championPrestigeDictionary = new ()
                {
                    items = new ChampionLevelItem[teamStats.Heroes.Count]
                },
                _lastOpened = DateTime.Now.ToLongTimeString()
            };

            for (int i = 0; i < teamStats.Heroes.Count; i++)
            {
                data.LevelDictionary.items[i] = new ChampionLevelItem
                {
                    championRole = teamStats.Heroes[i].GetRole(),
                    championLevel = teamStats.Heroes[i].HeroLevel
                };
                data.ChampionPrestigeDictionary.items[i] = new ChampionLevelItem
                {
                    championRole = teamStats.Heroes[i].GetRole(),
                    championLevel = teamStats.Heroes[i].PrestigeLevel
                };
            }
            
            return data;
        }
    }
}