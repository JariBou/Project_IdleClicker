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
        [SerializeField] private int target = 30;
        private LevelsManager _levelsManager;
        private EnemyBase _enemyBase;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = target;
        }
        private void Start()
        {
            _levelsManager = LevelsManager.Instance;
            _enemyBase = EnemyBase.Instance;
            JsonSaveData saveData;
            try
            {
                saveData = SaveManager.LoadFromJson(_saveName);
            }
            catch (Exception e)
            {
                TimePassedGUI.Instance.ButtonClick();
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
            long goldValue;
            if (_levelsManager.CurrentLevel == 0)
            {
                goldValue = (long)375 / 2 * (long)timeSpan.TotalMinutes * (long)(timeSpan.TotalMinutes / _enemyBase.SpawnRate * 5);
            }
            else
            {
                goldValue = (long)((((300 * _levelsManager.CurrentLevel) + (450 * _levelsManager.CurrentLevel)) / 2 + ((int)Mathf.Floor(_levelsManager.CurrentLevel - 1) / 5) * 150000) * (long)(timeSpan.TotalMinutes / _enemyBase.SpawnRate * 5)) ;

            }


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