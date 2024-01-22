using System;
using NaughtyAttributes;
using ProjectClicker.Core;
using ProjectClicker.Enemies;
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
            
            DateTime lastOpened = DateTime.Parse(saveData.LastOpened);

            TimeSpan timeSpan = DateTime.Now.Subtract(lastOpened);
            long goldValue = (long)(timeSpan.TotalMinutes * saveData.CurrLevel * 2);

            TimePassedGUI.Instance.Config(goldValue, timeSpan.TotalMinutes);

            LevelsManager.Instance.SetLevel(saveData.CurrLevel);
            EnemyBase.Instance.LoadSave();
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