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
        private PrestigeManager _prestigeManager;
        private String _championRole;
        [SerializeField] private UpgradeResource _upgradeResource;


        public void Initialize(int index, string championRole, UpgradeResource upgradeResource)
        {
            _championRole = championRole;
            _upgradeResource = upgradeResource;
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
            
            UpdateUpgradePanel();
            switch (_upgradeResource)
            {
                case UpgradeResource.GoldUpgrade:
                    _upgradeCostInt = 1240;
                    break;
                case UpgradeResource.PrestigeUpgrade:
                    _upgradeCostInt = 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _upgradeCost.text = Utils.NumberToString(_upgradeCostInt);

            _championStats.LinkedDisplays.Add(this);
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
            HeroesBehavior hero = _championStats;
            switch (_upgradeResource)
            {
                case UpgradeResource.GoldUpgrade:
                    _damageAmount.text = Utils.NumberToString((decimal)(hero.HeroLevel *
                        hero.Info.DmgPerLevel + hero.BaseDamage));
                    _healthAmount.text = Utils.NumberToString((decimal)(hero.HeroLevel *
                        hero.Info.HealthPerLevel + hero.BaseMaxHealth));

                    _armorOrHealAmount.text = _championRole == "Healer"
                        ? Utils.NumberToString((decimal)(hero.HeroLevel * hero.Info.HealStrengthPerLevel + hero.BaseHealStrength))
                        : Utils.NumberToString((decimal)(hero.HeroLevel * hero.Info.ArmorPerLevel + hero.BaseArmor));
                    _heroLevel.text = "Lvl " + hero.HeroLevel;
                    break;
                case UpgradeResource.PrestigeUpgrade:
                    _damageAmount.text = Utils.NumberToString((decimal)(hero.PrestigeLevel *
                        hero.Info.DmgPerLevel + hero.BaseDamage));
                    _healthAmount.text = Utils.NumberToString((decimal)(hero.PrestigeLevel *
                        hero.Info.HealthPerLevel + hero.BaseMaxHealth));

                    _armorOrHealAmount.text = _championRole == "Healer"
                        ? Utils.NumberToString((decimal)(hero.PrestigeLevel * hero.Info.HealStrengthPerLevel + hero.BaseHealStrength))
                        : Utils.NumberToString((decimal)(hero.PrestigeLevel * hero.Info.ArmorPerLevel + hero.BaseArmor));
                    _heroLevel.text = "Lvl " + hero.PrestigeLevel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _upgradeCost.text = Utils.NumberToString(_upgradeCostInt);
        }

        public void BuyUpgrade()
        {
            if (_upgradeResource == UpgradeResource.GoldUpgrade)
            {
                if (_goldManager == null) _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
                if (_goldManager.Gold < (ulong)_upgradeCostInt) return;
                
                _goldManager.RemoveGold((ulong)_upgradeCostInt);
                if (_championStats.HeroLevel >= 1) _upgradeCostInt += 1240 * _championStats.HeroLevel * 2;
                else _upgradeCostInt += 1240;
                _championStats.Upgrade();
                UpdateUpgradePanel();
            }
            else
            {
                if (_prestigeManager == null) _prestigeManager = GameObject.FindWithTag("Managers").GetComponent<PrestigeManager>();
                if (_prestigeManager.Medals < (uint)_upgradeCostInt) return;
                
                _prestigeManager.RemoveMedals(_upgradeCostInt);
                _upgradeCostInt = 4 + (int)(2.5f * (_championStats.PrestigeLevel * (_championStats.PrestigeLevel + 1)) / 2);
                _championStats.PrestigeUpgrade();
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
