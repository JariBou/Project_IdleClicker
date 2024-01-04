using ProjectClicker.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static ProjectClicker.HeroesBehavior;

namespace ProjectClicker
{
    public class HeroUpgradeDisplay : MonoBehaviour
    {
        [Header("Hero ")]
        [SerializeField] private Image _heroIcon;
        public TextMeshProUGUI _heroName;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI _damageAmount;
        [SerializeField] private TextMeshProUGUI _healthAmount;
        [SerializeField] private TextMeshProUGUI _armorOrHealAmount;

        [Header("Upgrade")]
        [SerializeField] private TextMeshProUGUI _upgradeCost;
        private int _upgradeCostInt;
        [SerializeField] private TextMeshProUGUI _heroLevel;
        [SerializeField] private Button _upgradeButton;

        [SerializeField] private Hero[] _heroesLibrary;

        [Header("Hero Stats")]
        private int HeroIndex;
        private HeroesBehavior championStats;
        /*        private TeamStats _teamStats;*/


        [Header("Buy Upgrade")]
        private GoldManager _goldManager;
        private String ChampionRole;


        public void Initialize(int index, string championRole)
        {
            ChampionRole = championRole;
            Debug.Log("Initializing the " + championRole + " panel");
            HeroIndex = GetHeroIndex(championRole);
            if (HeroIndex == -1)
            {
                Debug.LogError("Hero not found");
                return;
            }
            _heroIcon.sprite = _heroesLibrary[HeroIndex]._heroIcon;
            _heroName.text = _heroesLibrary[HeroIndex]._heroName;
            championStats = GameObject.FindWithTag("Team").transform?.GetChild(index).GetComponent<HeroesBehavior>();
            if (championStats == null) return;
            if (_goldManager == null) _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _damageAmount.text = _goldManager.NumberToString((decimal)championStats.Damage);
            _healthAmount.text = _goldManager.NumberToString((decimal)championStats.MaxHealth);
            if (championRole == "Healer") _armorOrHealAmount.text = _goldManager.NumberToString((decimal)championStats.PowerHeal);
            else _armorOrHealAmount.text = _goldManager.NumberToString((decimal)championStats.Armor);
            _heroLevel.text = "Lvl " + _goldManager.NumberToString((decimal)championStats.HeroLevel); ;
            _upgradeCostInt = 1240;
            _upgradeCost.text = _goldManager.NumberToString((decimal)_upgradeCostInt);

        }

        private void UpdateUpgradePanel()
        {
            _damageAmount.text = _goldManager.NumberToString((decimal)championStats.Damage);
            _healthAmount.text = _goldManager.NumberToString((decimal)championStats.MaxHealth);
            if (ChampionRole == "Healer") _armorOrHealAmount.text = _goldManager.NumberToString((decimal)championStats.PowerHeal);
            else _armorOrHealAmount.text = _goldManager.NumberToString((decimal)championStats.Armor);
            _heroLevel.text = "Lvl " + championStats.HeroLevel.ToString();
            _upgradeCost.text = _goldManager.NumberToString((decimal)_upgradeCostInt);

        }

        public void BuyUpgrade()
        {
            if (_goldManager == null) _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            if (_goldManager.gold < (ulong)_upgradeCostInt) return;
            else
            {
                 _goldManager.RemoveGold((ulong)_upgradeCostInt);
                if (championStats.HeroLevel >= 1) _upgradeCostInt += 1240 * championStats.HeroLevel;
                else _upgradeCostInt += 1240;
                championStats.Upgrade();
                UpdateUpgradePanel();
            }
        }

        private int GetHeroIndex(string championRole)
        {
            for (int i = 0; i < _heroesLibrary.Length; i++)
            {
                if (_heroesLibrary[i]._heroName == championRole)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    [Serializable]
    public struct Hero
    {
        
        public string _heroName;
        public Sprite _heroIcon;
    }
}
