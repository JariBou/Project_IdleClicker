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

            foreach (ChampionLevelItem item in saveData.LevelDictionary)
            {
                _teamStats.GetHeroByRole(item.championRole).SetLevel(item.championLevel);
            }
            
            
            LevelsManager.Instance.SetLevel(saveData.CurrLevel);

        }

        private void OnApplicationQuit()
        {
            SaveManager.SaveToJson(JsonSaveData.Initialise(), _saveName);
        }

        [Button]
        private void Save()
        {
            SaveManager.SaveToJson(JsonSaveData.Initialise(), _saveName);
        }
    }
}