using System;
using NaughtyAttributes;
using ProjectClicker.Core;
using UnityEngine;

namespace ProjectClicker.Saves
{
    public class SaveObject : MonoBehaviour
    {
        [SerializeField] private string _saveName = "GameSave";
        [SerializeField] private TeamStats _teamStats;
        private void Start()
        {
            JsonSaveData saveData;
            try
            {
                saveData = SaveManager.LoadFromJson(_saveName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }


            GoldManager.Instance.SetGold(saveData.Gold);

            PrestigeManager.Instance.PassData(saveData);

            for (int i = 0; i < saveData.LevelDictionary.items.Length; i++)
            {
                var levelPair = saveData.LevelDictionary.items[i];
                var prestigePair = saveData.ChampionPrestigeDictionary.items[i];
                _teamStats.GetHeroByRole(levelPair.championRole).SetLevels(levelPair.championLevel, prestigePair.championLevel);

            }
            foreach (ChampionLevelItem item in saveData.LevelDictionary)
            {
            }
            
            
            LevelsManager.Instance.SetLevel(saveData.CurrLevel);

        }

        private void OnApplicationQuit()
        {
            var f = JsonSaveData.Initialise();
            SaveManager.SaveToJson(f, _saveName);
        }

        [Button]
        private void Save()
        {
            var f = JsonSaveData.Initialise();
            SaveManager.SaveToJson(f, _saveName);
        }
    }
}