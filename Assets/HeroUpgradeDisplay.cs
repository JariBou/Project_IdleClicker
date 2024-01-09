using ProjectClicker.Core;
using System;
using ProjectClicker.Heroes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private int _heroIndex;
        private HeroesBehavior _championStats;
        /*        private TeamStats _teamStats;*/


        [Header("Buy Upgrade")]
        private GoldManager _goldManager;
        private String _championRole;


        public void Initialize(int index, string championRole)
        {
            _championRole = championRole;
            Debug.Log("Initializing the " + championRole + " panel");
            _heroIndex = GetHeroIndex(championRole);
            if (_heroIndex == -1)
            {
                Debug.LogError("Hero not found");
                return;
            }
            _heroIcon.sprite = _heroesLibrary[_heroIndex]._heroIcon;
            _heroName.text = _heroesLibrary[_heroIndex]._heroName;
            _championStats = GameObject.FindWithTag("Team").transform.GetChild(index).GetComponent<HeroesBehavior>();
            if (_championStats == null) return;
            if (_goldManager == null)
            {
                _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            }
            
            _damageAmount.text = _goldManager.NumberToString((decimal)_championStats.Damage);
            _healthAmount.text = _goldManager.NumberToString((decimal)_championStats.MaxHealth);
            
            _armorOrHealAmount.text = championRole == "Healer" ? _goldManager.NumberToString((decimal)_championStats.PowerHeal) 
                : _goldManager.NumberToString((decimal)_championStats.Armor);
            
            _heroLevel.text = "Lvl " + _goldManager.NumberToString(_championStats.HeroLevel); ;
            _upgradeCostInt = 1240;
            _upgradeCost.text = _goldManager.NumberToString(_upgradeCostInt);

            _championStats.LinkedDisplay = this;

        }

        private void OnEnable()
        {
            LevelsManager.OnPrestige += UpdateUpgradePanel;
        }
        
        private void OnDisable()
        {
            LevelsManager.OnPrestige -= UpdateUpgradePanel;
        }

        public void UpdateUpgradePanel()
        {
            _damageAmount.text = _goldManager.NumberToString((decimal)_championStats.Damage);
            _healthAmount.text = _goldManager.NumberToString((decimal)_championStats.MaxHealth);
            
            _armorOrHealAmount.text = _championRole == "Healer" ? _goldManager.NumberToString((decimal)_championStats.PowerHeal) 
                : _goldManager.NumberToString((decimal)_championStats.Armor);
            
            _heroLevel.text = "Lvl " + _championStats.HeroLevel;
            _upgradeCost.text = _goldManager.NumberToString(_upgradeCostInt);

        }

        public void BuyUpgrade()
        {
            if (_goldManager == null) _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            if (_goldManager.Gold < (ulong)_upgradeCostInt) return;
            
            _goldManager.RemoveGold((ulong)_upgradeCostInt);
            if (_championStats.HeroLevel >= 1) _upgradeCostInt += 1240 * _championStats.HeroLevel * 2;
            else _upgradeCostInt += 1240;
            _championStats.Upgrade();
            UpdateUpgradePanel();
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
