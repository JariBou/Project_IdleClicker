using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectClicker
{
    public class HeroUpgradeDisplay : MonoBehaviour
    {
        [SerializeField] private Image _heroIcon;
        [SerializeField] private TMP_Text _statsDisplay;
        [SerializeField] private TMP_Text _upgradesAmountDisplay;
        [SerializeField] private TMP_Text _upgradeCost;
        public int HeroIndex { get; private set; }
        private TeamStats _teamStats;

        public void Initialize(int index, TeamStats teamStats)
        {
            HeroIndex = index;
            _teamStats = teamStats;
        }

        public void Upgrade()
        {
            _teamStats.UpgradeHeroAtIndex(HeroIndex);
        }
    }
}
